// @vitest-environment jsdom

import { afterEach, beforeEach, describe, expect, test, vi } from 'vitest';

// Load the script once - it assigns window.twSkeleton on evaluation.
await import('../../../src/TwBlazor/wwwroot/js/twblazor.js');

// jsdom has no ResizeObserver and no real layout engine, so every test stubs
// getBoundingClientRect/getComputedStyle/Range.getClientRects directly on the elements under test,
// the same way the positionPanel tests above stub getBoundingClientRect on a mock panel.
class MockResizeObserver {
    constructor(callback) {
        this.callback = callback;
        this.disconnected = false;
        MockResizeObserver.instances.push(this);
    }

    observe(target) {
        this.target = target;
    }

    disconnect() {
        this.disconnected = true;
    }
}
MockResizeObserver.instances = [];

function rect(top, left, width, height) {
    return { top, left, right: left + width, bottom: top + height, width, height };
}

function mockLeaf(tag, r, extraStyle) {
    const el = document.createElement(tag);
    el.getBoundingClientRect = () => rect(r.top, r.left, r.width, r.height);
    if (extraStyle) {
        Object.assign(el.style, extraStyle);
    }
    document.body.appendChild(el);
    return el;
}

describe('twSkeleton', () => {
    let container;

    beforeEach(() => {
        MockResizeObserver.instances = [];
        vi.stubGlobal('ResizeObserver', MockResizeObserver);
        container = document.createElement('div');
        container.getBoundingClientRect = () => rect(0, 0, 300, 300);
        document.body.appendChild(container);
    });

    afterEach(() => {
        window.twSkeleton.unobserve(container);
        document.body.innerHTML = '';
        vi.unstubAllGlobals();
        vi.restoreAllMocks();
        delete Range.prototype.getClientRects;
    });

    describe('observe / unobserve', () => {
        test('does nothing when container is null', () => {
            expect(() => window.twSkeleton.observe(null, { invokeMethodAsync: vi.fn() })).not.toThrow();
            expect(MockResizeObserver.instances).toHaveLength(0);
        });

        test('registers a ResizeObserver and emits an initial measurement', () => {
            const child = mockLeaf('div', { top: 10, left: 5, width: 40, height: 20 });
            container.appendChild(child);
            const dotnetRef = { invokeMethodAsync: vi.fn() };

            window.twSkeleton.observe(container, dotnetRef);

            expect(MockResizeObserver.instances).toHaveLength(1);
            expect(dotnetRef.invokeMethodAsync).toHaveBeenCalledTimes(1);
            expect(dotnetRef.invokeMethodAsync).toHaveBeenCalledWith('OnRectsMeasured', [
                expect.objectContaining({ top: 10, left: 5, width: 40, height: 20, shape: 'rect' })
            ]);
        });

        test('is idempotent for a container that is already observed', () => {
            const dotnetRef = { invokeMethodAsync: vi.fn() };

            window.twSkeleton.observe(container, dotnetRef);
            window.twSkeleton.observe(container, dotnetRef);

            expect(MockResizeObserver.instances).toHaveLength(1);
            expect(dotnetRef.invokeMethodAsync).toHaveBeenCalledTimes(1);
        });

        test('re-measures and reports again when the ResizeObserver fires', () => {
            const dotnetRef = { invokeMethodAsync: vi.fn() };

            window.twSkeleton.observe(container, dotnetRef);
            MockResizeObserver.instances[0].callback();

            expect(dotnetRef.invokeMethodAsync).toHaveBeenCalledTimes(2);
        });

        test('swallows errors thrown by invokeMethodAsync', () => {
            const error = new Error('interop error');
            const dotnetRef = { invokeMethodAsync: vi.fn().mockImplementation(() => { throw error; }) };
            const consoleErrorSpy = vi.spyOn(console, 'error').mockImplementation(() => {});

            expect(() => window.twSkeleton.observe(container, dotnetRef)).not.toThrow();
            expect(consoleErrorSpy).toHaveBeenCalledWith('twSkeleton emit error', error);
        });

        test('disconnects the observer and allows re-observing afterwards', () => {
            window.twSkeleton.observe(container, { invokeMethodAsync: vi.fn() });

            window.twSkeleton.unobserve(container);

            expect(MockResizeObserver.instances[0].disconnected).toBe(true);

            window.twSkeleton.observe(container, { invokeMethodAsync: vi.fn() });
            expect(MockResizeObserver.instances).toHaveLength(2);
        });

        test('unobserve does nothing when container was never observed', () => {
            expect(() => window.twSkeleton.unobserve(container)).not.toThrow();
            expect(() => window.twSkeleton.unobserve(null)).not.toThrow();
        });
    });

    describe('shape detection', () => {
        test('_isVisible is false for display:none', () => {
            const el = mockLeaf('div', { top: 0, left: 0, width: 10, height: 10 }, { display: 'none' });
            expect(window.twSkeleton._isVisible(el)).toBe(false);
        });

        test('_isVisible is false for a zero-size element', () => {
            const el = mockLeaf('div', { top: 0, left: 0, width: 0, height: 0 });
            expect(window.twSkeleton._isVisible(el)).toBe(false);
        });

        test('_isVisible is true for a normal, sized element', () => {
            const el = mockLeaf('div', { top: 0, left: 0, width: 10, height: 10 });
            expect(window.twSkeleton._isVisible(el)).toBe(true);
        });

        test('_isRound is true when every corner is at least half the shorter side', () => {
            const el = mockLeaf('div', { top: 0, left: 0, width: 40, height: 40 }, {
                borderTopLeftRadius: '9999px',
                borderTopRightRadius: '9999px',
                borderBottomLeftRadius: '9999px',
                borderBottomRightRadius: '9999px'
            });
            expect(window.twSkeleton._isRound(el, el.getBoundingClientRect())).toBe(true);
        });

        test('_isRound is false for a merely-rounded rectangle', () => {
            const el = mockLeaf('div', { top: 0, left: 0, width: 100, height: 40 }, {
                borderTopLeftRadius: '4px',
                borderTopRightRadius: '4px',
                borderBottomLeftRadius: '4px',
                borderBottomRightRadius: '4px'
            });
            expect(window.twSkeleton._isRound(el, el.getBoundingClientRect())).toBe(false);
        });
    });

    describe('_measure', () => {
        test('walks into a structural wrapper instead of treating it as one box', () => {
            const wrapper = document.createElement('div');
            wrapper.getBoundingClientRect = () => rect(0, 0, 200, 100);
            container.appendChild(wrapper);
            const leaf = mockLeaf('img', { top: 5, left: 5, width: 50, height: 50 });
            wrapper.appendChild(leaf);

            const result = window.twSkeleton._measure(container);

            expect(result).toEqual([
                expect.objectContaining({ top: 5, left: 5, width: 50, height: 50, shape: 'rect' })
            ]);
        });

        test('reports a leaf whose computed border-radius makes it round as shape "circle"', () => {
            const avatar = mockLeaf('div', { top: 0, left: 0, width: 48, height: 48 }, {
                borderTopLeftRadius: '9999px',
                borderTopRightRadius: '9999px',
                borderBottomLeftRadius: '9999px',
                borderBottomRightRadius: '9999px'
            });
            container.appendChild(avatar);

            const result = window.twSkeleton._measure(container);

            expect(result).toEqual([expect.objectContaining({ shape: 'circle' })]);
        });

        test('reports a round leaf with text content (e.g. initials) as a single circle, not text lines', () => {
            const avatar = mockLeaf('div', { top: 0, left: 0, width: 48, height: 48 }, {
                borderTopLeftRadius: '9999px',
                borderTopRightRadius: '9999px',
                borderBottomLeftRadius: '9999px',
                borderBottomRightRadius: '9999px'
            });
            avatar.textContent = 'JD';
            container.appendChild(avatar);

            const getClientRectsSpy = vi.fn(() => [rect(14, 14, 20, 20)]);
            Range.prototype.getClientRects = getClientRectsSpy;

            const result = window.twSkeleton._measure(container);

            expect(getClientRectsSpy).not.toHaveBeenCalled();
            expect(result).toEqual([
                expect.objectContaining({ top: 0, left: 0, width: 48, height: 48, shape: 'circle' })
            ]);
        });

        test('splits a text leaf into one rect per wrapped line via Range.getClientRects', () => {
            const paragraph = mockLeaf('p', { top: 0, left: 0, width: 300, height: 40 });
            paragraph.textContent = 'Some lorem ipsum text that wraps onto two lines';
            container.appendChild(paragraph);

            // jsdom's Range doesn't implement getClientRects at all, so there's nothing for vi.spyOn to
            // wrap - it has to be assigned directly, and cleaned up manually since restoreAllMocks only
            // undoes spies.
            Range.prototype.getClientRects = () => [
                rect(0, 0, 300, 20),
                rect(20, 0, 120, 20)
            ];

            const result = window.twSkeleton._measure(container);

            expect(result).toEqual([
                expect.objectContaining({ top: 0, left: 0, width: 300, height: 20, shape: 'text' }),
                expect.objectContaining({ top: 20, left: 0, width: 120, height: 20, shape: 'text' })
            ]);
        });

        test('skips invisible and zero-size children', () => {
            container.appendChild(mockLeaf('div', { top: 0, left: 0, width: 10, height: 10 }, { display: 'none' }));
            container.appendChild(mockLeaf('div', { top: 0, left: 0, width: 0, height: 0 }));

            const result = window.twSkeleton._measure(container);

            expect(result).toEqual([]);
        });

        test('ignores whitespace-only text nodes when walking a leaf for line rects', () => {
            const paragraph = mockLeaf('p', { top: 0, left: 0, width: 300, height: 20 });
            // A leading whitespace-only text node (common between elements in real, indented markup)
            // must not produce its own Range/line rect - only the real text node should.
            paragraph.appendChild(document.createTextNode('   \n   '));
            paragraph.appendChild(document.createTextNode('Real text'));
            container.appendChild(paragraph);

            const getClientRectsSpy = vi.fn(() => [rect(0, 0, 300, 20)]);
            Range.prototype.getClientRects = getClientRectsSpy;

            const result = window.twSkeleton._measure(container);

            expect(getClientRectsSpy).toHaveBeenCalledTimes(1);
            expect(result).toEqual([expect.objectContaining({ shape: 'text' })]);
        });

        test('skips a zero-size line rect returned alongside real ones', () => {
            // Range.getClientRects() can include a zero-size rect at a line-wrap boundary in real
            // browsers; it must not produce its own (empty) skeleton bar.
            const paragraph = mockLeaf('p', { top: 0, left: 0, width: 300, height: 20 });
            paragraph.textContent = 'Real text';
            container.appendChild(paragraph);
            Range.prototype.getClientRects = () => [
                rect(0, 0, 0, 0),
                rect(0, 0, 300, 20)
            ];

            const result = window.twSkeleton._measure(container);

            expect(result).toEqual([
                expect.objectContaining({ top: 0, left: 0, width: 300, height: 20, shape: 'text' })
            ]);
        });

        test('falls back to a single whole-element box when a text leaf yields no line rects', () => {
            // E.g. text that is present in the DOM but produces no visible line boxes - the leaf itself
            // is still measured and reported as one generic box instead of being dropped entirely.
            const paragraph = mockLeaf('p', { top: 0, left: 0, width: 300, height: 20 });
            paragraph.textContent = 'Real text';
            container.appendChild(paragraph);
            Range.prototype.getClientRects = () => [];

            const result = window.twSkeleton._measure(container);

            expect(result).toEqual([
                expect.objectContaining({ top: 0, left: 0, width: 300, height: 20, shape: 'rect' })
            ]);
        });
    });
});
