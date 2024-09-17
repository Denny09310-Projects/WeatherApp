/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        './**/*{cs,razor}',
        './wwwroot/index.html'
    ],
    theme: {
        extend: {},
    },
    daisyui: {
        themes: [
            {
                black: {
                    ...require("daisyui/src/theming/themes")["black"],
                    "--rounded-box": "1rem",
                    "--rounded-btn": "0.5rem",
                },
            }
        ]
    },
    plugins: [
        require('tailwindcss-animate'),
        require('daisyui')
    ],
}

