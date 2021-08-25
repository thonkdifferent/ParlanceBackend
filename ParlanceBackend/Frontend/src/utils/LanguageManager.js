import Fetch from "./Fetch";

class LanguageManager {
    languages;

    constructor() {
        this.languages = [];

        this.updateLanguages();
    }

    async updateLanguages() {
        this.languages = (await Fetch.get("/languages")).reduce((languages, language) => {
            languages[language.identifier] = language;
            return languages;
        }, {});
    }
}

let languageManager = new LanguageManager();
export default languageManager;