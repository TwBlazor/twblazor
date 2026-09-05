import { fileURLToPath } from 'node:url';
import { defineConfig } from 'vitest/config';

// Root is pinned to the repository root (this file's parent directory) so test
// discovery and coverage paths resolve the same way regardless of the working
// directory `vitest` is invoked from.
const repoRoot = fileURLToPath(new URL('..', import.meta.url));

export default defineConfig({
    root: repoRoot,
    test: {
        environment: 'jsdom',
        include: ['tests/TwBlazor.Tests/js/**/*.test.js'],
        coverage: {
            provider: 'v8',
            include: ['src/TwBlazor/wwwroot/js/**/*.js'],
            reporter: ['lcov', 'text'],
            reportsDirectory: 'coverage',
        },
    },
});
