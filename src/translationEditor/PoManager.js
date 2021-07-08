import gettextParser from "gettext-parser";
import EventEmitter from "eventemitter3";
import tags from "language-tags";

class PoManager extends EventEmitter {
    poData;
    languageTag;
    hasError;

    constructor(fileContents, languageTag) {
        super();

        this.languageTag = languageTag;

        try {
            this.poData = gettextParser.po.parse(fileContents)
            delete this.poData.translations[""];
            this.hasError = false;
        } catch {
            this.hasError = true;
        }
    }

    getTargetName() {
        return tags(this.languageTag.replace("_", "-")).language().descriptions()[0];
    }

    contexts() {
        return Object.keys(this.poData.translations);
    }

    getKeys(context) {
        return Object.keys(this.poData.translations[context]);
    }
    
    getTranslation(context, key) {
        return this.poData.translations[context][key];
    }

    setTranslation(context, key, index, translation) {
        this.poData.translations[context][key].msgstr[index] = translation;
        this.emit("translationsChanged");
    }
}

export default PoManager;