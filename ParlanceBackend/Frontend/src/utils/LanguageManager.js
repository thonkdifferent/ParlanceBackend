import Fetch from "./Fetch";

class LanguageManager {
    languages;

    constructor() {
        this.languages = [];

        this.updateLanguages();
    }

    async updateLanguages() {
        this.languages = (await Fetch.get("/languages")).reduce((languages, language) => {
            languages[language.identifier.toLowerCase()] = language;
            return languages;
        }, {});
    }
    
    getLanguage(lang) {
        for (let transformed of [
            lang.toLowerCase(),
            lang.toLowerCase().replace("_", "-")
        ]) {
            if (this.languages[transformed]) return this.languages[transformed];
        }
        return null;
    }
}

let languageManager = new LanguageManager();
export default languageManager;