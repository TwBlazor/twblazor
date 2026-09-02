import { defineConfig } from 'vitest/config';

export default defineConfig({
    test: {
        environment: 'jsdom',
        include: ['TwBlazor.Tests/js/**/*.test.js'],
        coverage: {
            provider: 'v8',
            include: ['TwBlazor/wwwroot/js/**/*.js'],
            reporter: ['lcov', 'text'],
            reportsDirectory: './coverage',
        },
    },
});
