import gettextParser from "gettext-parser";
import EventEmitter from "eventemitter3";
import tags from "language-tags";
import checks from "./Checks";

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

            this.updateChecks();
        } catch (error) {
            this.hasError = error;
        }
    }

    updateChecks() {
        this.contexts().forEach(context => this.getKeys(context).forEach(key => {
            this.updateCheck(context, key);
        }));
    }

    updateCheck(context, key) {
        let translation = this.getTranslation(context, key);
        translation.checks = translation.msgstr.map(str => str === "" ? [] : checks.map(checkFunction => checkFunction(translation.msgid, str)).filter(item => item != null));
        

        // translation.checks.push({
        //     title: "Test",
        //     message: "This is a test check. Please ignore it."
        // });
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

        this.updateCheck(context, key);
    }

    nextSelection(currentContext, currentKey) {
        let next = false;
        for (let context of this.contexts()) {
            for (let key of this.getKeys(context)) {
                if (next) return {
                    context,
                    key
                }

                if (context === currentContext && key === currentKey) next = true;
            }
        }

        return {
            context: this.contexts()[0],
            key: this.getKeys(this.contexts()[0])[0]
        }
    }

    previousSelection(currentContext, currentKey) {
        let current = {
            context: this.contexts()[this.contexts().length - 1],
            key: this.getKeys(this.contexts()[this.contexts().length - 1])[this.getKeys(this.contexts()[this.contexts().length - 1]).length - 1]
        }

        for (let context of this.contexts()) {
            for (let key of this.getKeys(context)) {
                if (context === currentContext && key === currentKey) return current;
                current = {
                    context, key
                }
            }
        }

        return current;
    }
}

export default PoManager;