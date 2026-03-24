declare const _default: {
    darkMode: ["class"];
    content: string[];
    theme: {
        extend: {
            colors: {
                midnight: string;
                ink: string;
                cosmic: string;
                aurora: string;
                wine: string;
                gold: string;
                moon: string;
                mist: string;
                starlight: string;
            };
            fontFamily: {
                display: [string, string];
                sans: [string, string];
            };
            boxShadow: {
                oracle: string;
                glow: string;
            };
            backgroundImage: {
                nebula: string;
            };
            keyframes: {
                float: {
                    "0%, 100%": {
                        transform: string;
                    };
                    "50%": {
                        transform: string;
                    };
                };
                pulseGlow: {
                    "0%, 100%": {
                        opacity: string;
                        transform: string;
                    };
                    "50%": {
                        opacity: string;
                        transform: string;
                    };
                };
                drift: {
                    "0%": {
                        transform: string;
                    };
                    "50%": {
                        transform: string;
                    };
                    "100%": {
                        transform: string;
                    };
                };
            };
            animation: {
                float: string;
                pulseGlow: string;
                drift: string;
            };
        };
    };
    plugins: any[];
};
export default _default;
