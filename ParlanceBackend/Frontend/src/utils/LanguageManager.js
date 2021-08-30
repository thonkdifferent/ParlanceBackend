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
    
    sortedLanguages() {
        return Object.values(this.languages).sort((first, second) => first.name > second.name);
    }
    
    getLanguage(lang) {
        let transformations = [
            lang.toLowerCase(),
            lang.toLowerCase().replace("_", "-")
        ];
        
        for (let transformed of transformations) {
            if (this.languages[transformed]) return this.languages[transformed];
        }
        
        //Now match by language code only
        for (let knownLanguage of Object.keys(this.languages)) {
            for (let transformed of transformations) {
                if (knownLanguage.startsWith(transformed)) return this.languages[knownLanguage];
            }
        }
        
        return null;
    }
}

let languageManager = new LanguageManager();
export default languageManager;