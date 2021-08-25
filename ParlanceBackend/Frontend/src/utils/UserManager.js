import EventEmitter from "eventemitter3";
import Fetch from './Fetch';

import CryptoJS from "crypto-js";

import Modal from "../components/Modal";
import LoginModal from "../modals/LoginModal";
import UserProfileModal from "../modals/UserProfileModal";

class UserManager extends EventEmitter {
    #username;
    #email;
    #isSuperuser;

    constructor() {
        super();

        this.#username = "";
        this.#email = "";
        this.#isSuperuser = false;

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

    async openLoginModal() {
        if (this.loggedIn()) {
            Modal.mount(<UserProfileModal />)
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
                this.#isSuperuser = permissionData.superuser;
            } catch {
                //The token is invalid
                await this.logout();
            }
        } else {
            this.#username = "";
            this.#email = "";
            this.#isSuperuser = false;
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

    profilePictureUrl() {
        let normalised = this.#email.trim().toLowerCase();
        let md5 = CryptoJS.MD5(normalised);
        return `https://www.gravatar.com/avatar/${md5}`;
    }
}

let userManager = new UserManager();
export default userManager;