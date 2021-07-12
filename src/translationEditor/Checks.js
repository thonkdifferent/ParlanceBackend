const checks = [
    function CheckTrailingStop(source, translation) {
        if (source.endsWith(".") && !translation.endsWith(".")) {
            return {
                "title": "Translation missing trailing stop",
                "message": "The translation is missing a full stop at the end of the string",
                "fixup": `${translation}.`
            }
        }

        if (!source.endsWith(".") && translation.endsWith(".")) {
            return {
                "title": "Extraenuous full stop",
                "message": "The translation contains an extra full stop at the end of the string",
                "fixup": `${translation.substr(0, translation.length - 1)}`
            }
        }
    },
    function CheckAllCaps(source, translation) {
        if (source.toUpperCase() === source && translation.toUpperCase() !== translation) {
            return {
                "title": "All Caps",
                "message": "The translation should also be in all caps",
                "fixup": translation.toUpperCase()
            }
        }
    },
    function CheckNumeralReplacement(source, translation) {
        if (source.includes("%n") && !translation.includes("%n")) {
            return {
                "title": "Missing Placeholder",
                "message": "The translation needs to contain a %n placeholder."
            }
        }
    },
    function CheckStringReplacement(source, translation) {
        for (let i = 1; i < 10; i++) {
            if (source.includes(`%${i}`) && !translation.includes(`%${i}`)) {
                return {
                    "title": "Missing Placeholder",
                    "message": `The translation needs to contain a %${i} placeholder.`
                }
            }
        }
    }
];

export default checks;