export default {
    darkMode: ["class"],
    content: ["./index.html", "./src/**/*.{ts,tsx}"],
    theme: {
        extend: {
            colors: {
                midnight: "#080610",
                ink: "#0f0b18",
                cosmic: "#171126",
                aurora: "#281d3d",
                wine: "#4a203c",
                gold: "#e5c98f",
                moon: "#f4ecda",
                mist: "#c6b9dd",
                starlight: "#97d4ff"
            },
            fontFamily: {
                display: ["Cormorant Garamond", "serif"],
                sans: ["Manrope", "sans-serif"]
            },
            boxShadow: {
                oracle: "0 20px 80px rgba(16, 10, 34, 0.55)",
                glow: "0 0 0 1px rgba(229, 201, 143, 0.18), 0 18px 50px rgba(81, 49, 108, 0.22)"
            },
            backgroundImage: {
                nebula: "radial-gradient(circle at top, rgba(151, 212, 255, 0.12), transparent 32%), radial-gradient(circle at 20% 20%, rgba(129, 86, 196, 0.22), transparent 28%), radial-gradient(circle at 80% 15%, rgba(229, 201, 143, 0.12), transparent 22%), linear-gradient(135deg, #06040d 0%, #0e0a17 45%, #1a1227 100%)"
            },
            keyframes: {
                float: {
                    "0%, 100%": { transform: "translateY(0px)" },
                    "50%": { transform: "translateY(-12px)" }
                },
                pulseGlow: {
                    "0%, 100%": { opacity: "0.5", transform: "scale(1)" },
                    "50%": { opacity: "0.9", transform: "scale(1.06)" }
                },
                drift: {
                    "0%": { transform: "translate3d(0, 0, 0)" },
                    "50%": { transform: "translate3d(16px, -18px, 0)" },
                    "100%": { transform: "translate3d(0, 0, 0)" }
                }
            },
            animation: {
                float: "float 6s ease-in-out infinite",
                pulseGlow: "pulseGlow 6s ease-in-out infinite",
                drift: "drift 12s ease-in-out infinite"
            }
        }
    },
    plugins: []
};
