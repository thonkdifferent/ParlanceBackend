import EventEmitter from "eventemitter3";
import Fetch from './Fetch';

import CryptoJS from "crypto-js";

import Modal from "../components/Modal";
import LoginModal from "../modals/LoginModal";
import UserProfileModal from "../modals/UserProfileModal";
import languageManager from "./LanguageManager";

class UserManager extends EventEmitter {
    #username;
    #email;
    #isSuperuser;
    #allowedLanguages;
    #verified;

    constructor() {
        super();

        this.#username = "";
        this.#email = "";
        this.#isSuperuser = false;
        this.#allowedLanguages = {};
        this.#verified = false;

        this.refreshUserData();
    }

    async logout() {
        window.localStorage.removeItem("token");
        await this.refreshUserData();
    }

    async performLogin(token) {
        window.localStorage.setItem("token", token);
        await this.refreshUserData();
    }

    async openLoginModal(history) {
        if (this.loggedIn()) {
            Modal.mount(<UserProfileModal history={history} />)
        } else {
            await new Promise((res, rej) => {
                Modal.mount(<LoginModal />)
            })
        }
    }

    async refreshUserData() {
        let token = window.localStorage.getItem("token");
        if (token) {
            try {
                let [userData, permissionData] = await Promise.all([
                    Fetch.get("/users/me"),
                    Fetch.get("/users/me/permissions")
                ]);

                this.#username = userData.username;
                this.#email = userData.email;
                this.#verified = userData.verified;
                this.#isSuperuser = permissionData.superuser;
                this.#allowedLanguages = permissionData.allowedLanguages.reduce((languages, language) => {
                    languages[language.identifier.toLowerCase()] = language;
                    return languages;
                }, {});
            } catch {
                //The token is invalid
                await this.logout();
            }
        } else {
            this.#username = "";
            this.#email = "";
            this.#isSuperuser = false;
            this.#allowedLanguages = {};
            this.#verified = false;
        }
        this.emit("userChanged");
    }

    loggedIn() {
        return window.localStorage.getItem("token") !== null;
    }

    username() {
        return this.#username;
    }

    isSuperuser() {
        return this.#isSuperuser;
    }
    
    emailAddress() {
        return this.#email;
    }
    
    isVerified() {
        return this.#verified;
    }
    
    allowedLanguages() {
        if (this.isSuperuser()) {
            return languageManager.languages;
        } else {
            return this.#allowedLanguages;
        }
    }

    profilePictureUrl() {
        let normalised = this.#email.trim().toLowerCase();
        let md5 = CryptoJS.MD5(normalised);
        return `https://www.gravatar.com/avatar/${md5}`;
    }
}

let userManager = new UserManager();
export default userManager;