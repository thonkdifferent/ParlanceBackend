import i18n from 'i18next';
import { initReactI18next } from 'react-i18next';
import i18nHttpBackend from 'i18next-http-backend';

i18n.use(initReactI18next)
    .use(i18nHttpBackend)
    .init({
        backend: {
            loadPath: "/translations/{{lng}}/{{ns}}.json"
        },
        lng: "en"
    })