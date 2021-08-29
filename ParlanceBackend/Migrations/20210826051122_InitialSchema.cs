using Microsoft.EntityFrameworkCore.Migrations;

namespace ParlanceBackend.Migrations
{
    public partial class InitialSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Identifier = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Identifier);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Name = table.Column<string>(type: "text", nullable: false),
                    GitCloneUrl = table.Column<string>(type: "text", nullable: true),
                    Branch = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Superusers",
                columns: table => new
                {
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Superusers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AllowedLanguages",
                columns: table => new
                {
                    PermissionId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    LanguageIdentifier = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllowedLanguages", x => x.PermissionId);
                    table.ForeignKey(
                        name: "FK_AllowedLanguages_Languages_LanguageIdentifier",
                        column: x => x.LanguageIdentifier,
                        principalTable: "Languages",
                        principalColumn: "Identifier",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[,]
                {
                    { "af-ZA", "Afrikaans (South Africa)" },
                    { "ky-KG", "Kyrgyz (Kyrgyzstan)" },
                    { "lb-LU", "Luxembourgish (Luxembourg)" },
                    { "lo-LA", "Lao (Laos)" },
                    { "lt-LT", "Lithuanian (Lithuania)" },
                    { "lv-LV", "Latvian (Latvia)" },
                    { "mi-NZ", "Maori (New Zealand)" },
                    { "mk-MK", "Macedonian (North Macedonia)" },
                    { "ml-IN", "Malayalam (India)" },
                    { "mn-MN", "Mongolian (Mongolia)" },
                    { "kok-IN", "Konkani (India)" },
                    { "mr-IN", "Marathi (India)" },
                    { "ms-MY", "Malay (Malaysia)" },
                    { "mt-MT", "Maltese (Malta)" },
                    { "my-MM", "Burmese (Myanmar [Burma])" },
                    { "nb-NO", "Norwegian Bokmål (Norway)" },
                    { "ne-IN", "Nepali (India)" },
                    { "ne-NP", "Nepali (Nepal)" },
                    { "nl-BE", "Dutch (Belgium)" },
                    { "nl-NL", "Dutch (Netherlands)" },
                    { "nn-NO", "Norwegian Nynorsk (Norway)" },
                    { "ms-BN", "Malay (Brunei)" },
                    { "ko-KR", "Korean (South Korea)" },
                    { "kn-IN", "Kannada (India)" },
                    { "km-KH", "Khmer (Cambodia)" },
                    { "gl-ES", "Galician (Spain)" },
                    { "gsw-FR", "Swiss German (France)" },
                    { "gu-IN", "Gujarati (India)" },
                    { "haw-US", "Hawaiian (United States)" },
                    { "he-IL", "Hebrew (Israel)" },
                    { "hi-IN", "Hindi (India)" },
                    { "hr-BA", "Croatian (Bosnia & Herzegovina)" },
                    { "hr-HR", "Croatian (Croatia)" },
                    { "hsb-DE", "Upper Sorbian (Germany)" },
                    { "hu-HU", "Hungarian (Hungary)" },
                    { "hy-AM", "Armenian (Armenia)" },
                    { "id-ID", "Indonesian (Indonesia)" },
                    { "ig-NG", "Igbo (Nigeria)" },
                    { "ii-CN", "Sichuan Yi (China)" },
                    { "is-IS", "Icelandic (Iceland)" },
                    { "it-CH", "Italian (Switzerland)" },
                    { "it-IT", "Italian (Italy)" },
                    { "ja-JP", "Japanese (Japan)" },
                    { "ka-GE", "Georgian (Georgia)" },
                    { "kk-KZ", "Kazakh (Kazakhstan)" },
                    { "kl-GL", "Kalaallisut (Greenland)" },
                    { "om-ET", "Oromo (Ethiopia)" },
                    { "or-IN", "Odia (India)" },
                    { "pa-PK", "Punjabi (Arabic, Pakistan)" },
                    { "pl-PL", "Polish (Poland)" },
                    { "sv-FI", "Swedish (Finland)" },
                    { "sv-SE", "Swedish (Sweden)" },
                    { "sw-KE", "Swahili (Kenya)" },
                    { "ta-IN", "Tamil (India)" },
                    { "ta-LK", "Tamil (Sri Lanka)" },
                    { "te-IN", "Telugu (India)" },
                    { "th-TH", "Thai (Thailand)" },
                    { "ti-ER", "Tigrinya (Eritrea)" },
                    { "ti-ET", "Tigrinya (Ethiopia)" },
                    { "tk-TM", "Turkmen (Turkmenistan)" },
                    { "tr-TR", "Turkish (Turkey)" },
                    { "tt-RU", "Tatar (Russia)" },
                    { "ug-CN", "Uyghur (China)" },
                    { "uk-UA", "Ukrainian (Ukraine)" },
                    { "ur-IN", "Urdu (India)" },
                    { "ur-PK", "Urdu (Pakistan)" },
                    { "uz-UZ", "Uzbek (Cyrillic, Uzbekistan)" },
                    { "vi-VN", "Vietnamese (Vietnam)" },
                    { "wo-SN", "Wolof (Senegal)" },
                    { "xh-ZA", "Xhosa (South Africa)" },
                    { "yi-001", "Yiddish (World)" },
                    { "sr-RS", "Serbian (Cyrillic, Serbia)" },
                    { "gd-GB", "Scottish Gaelic (United Kingdom)" },
                    { "sr-ME", "Serbian (Cyrillic, Montenegro)" },
                    { "sq-AL", "Albanian (Albania)" },
                    { "ps-AF", "Pashto (Afghanistan)" },
                    { "pt-BR", "Portuguese (Brazil)" },
                    { "pt-PT", "Portuguese (Portugal)" },
                    { "rm-CH", "Romansh (Switzerland)" },
                    { "ro-MD", "Romanian (Moldova)" },
                    { "ro-RO", "Romanian (Romania)" },
                    { "ru-MD", "Russian (Moldova)" },
                    { "ru-RU", "Russian (Russia)" },
                    { "rw-RW", "Kinyarwanda (Rwanda)" },
                    { "sa-IN", "Sanskrit (India)" },
                    { "sah-RU", "Yakut (Russia)" },
                    { "sd-PK", "Sindhi (Arabic, Pakistan)" },
                    { "sd-IN", "Sindhi (Devanagari, India)" },
                    { "se-FI", "Northern Sami (Finland)" },
                    { "se-NO", "Northern Sami (Norway)" },
                    { "se-SE", "Northern Sami (Sweden)" },
                    { "si-LK", "Sinhala (Sri Lanka)" },
                    { "sk-SK", "Slovak (Slovakia)" },
                    { "sl-SI", "Slovenian (Slovenia)" },
                    { "smn-FI", "Inari Sami (Finland)" },
                    { "so-SO", "Somali (Somalia)" },
                    { "sr-BA", "Serbian (Cyrillic, Bosnia & Herzegovina)" },
                    { "ga-IE", "Irish (Ireland)" },
                    { "fy-NL", "Western Frisian (Netherlands)" },
                    { "fr-SN", "French (Senegal)" },
                    { "bs-BA", "Bosnian (Cyrillic, Bosnia & Herzegovina)" },
                    { "ca-ES", "Catalan (Spain)" },
                    { "cs-CZ", "Czech (Czechia)" },
                    { "cy-GB", "Welsh (United Kingdom)" },
                    { "da-DK", "Danish (Denmark)" },
                    { "de-AT", "German (Austria)" },
                    { "de-CH", "German (Switzerland)" },
                    { "de-DE", "German (Germany)" },
                    { "de-LI", "German (Liechtenstein)" },
                    { "de-LU", "German (Luxembourg)" },
                    { "dsb-DE", "Lower Sorbian (Germany)" },
                    { "dz-BT", "Dzongkha (Bhutan)" },
                    { "el-GR", "Greek (Greece)" },
                    { "en-AU", "English (Australia)" },
                    { "en-BZ", "English (Belize)" },
                    { "en-CA", "English (Canada)" },
                    { "en-GB", "English (United Kingdom)" },
                    { "en-HK", "English (Hong Kong SAR China)" },
                    { "en-IE", "English (Ireland)" },
                    { "en-IN", "English (India)" },
                    { "en-JM", "English (Jamaica)" },
                    { "br-FR", "Breton (France)" },
                    { "en-MY", "English (Malaysia)" },
                    { "bo-CN", "Tibetan (China)" },
                    { "bn-BD", "Bengali (Bangladesh)" },
                    { "am-ET", "Amharic (Ethiopia)" },
                    { "ar-AE", "Arabic (United Arab Emirates)" },
                    { "ar-BH", "Arabic (Bahrain)" },
                    { "ar-DZ", "Arabic (Algeria)" },
                    { "ar-EG", "Arabic (Egypt)" },
                    { "ar-IQ", "Arabic (Iraq)" },
                    { "ar-JO", "Arabic (Jordan)" },
                    { "ar-KW", "Arabic (Kuwait)" },
                    { "ar-LB", "Arabic (Lebanon)" },
                    { "ar-LY", "Arabic (Libya)" },
                    { "ar-MA", "Arabic (Morocco)" },
                    { "ar-OM", "Arabic (Oman)" },
                    { "ar-QA", "Arabic (Qatar)" },
                    { "ar-SA", "Arabic (Saudi Arabia)" },
                    { "ar-SY", "Arabic (Syria)" },
                    { "ar-TN", "Arabic (Tunisia)" },
                    { "ar-YE", "Arabic (Yemen)" },
                    { "as-IN", "Assamese (India)" },
                    { "az-AZ", "Azerbaijani (Cyrillic, Azerbaijan)" },
                    { "be-BY", "Belarusian (Belarus)" },
                    { "bg-BG", "Bulgarian (Bulgaria)" },
                    { "bn-IN", "Bengali (India)" },
                    { "yo-NG", "Yoruba (Nigeria)" },
                    { "en-NZ", "English (New Zealand)" },
                    { "en-SG", "English (Singapore)" },
                    { "es-VE", "Spanish (Venezuela)" },
                    { "et-EE", "Estonian (Estonia)" },
                    { "eu-ES", "Basque (Spain)" },
                    { "fa-IR", "Persian (Iran)" },
                    { "ff-SN", "Fulah (Latin, Senegal)" },
                    { "fi-FI", "Finnish (Finland)" },
                    { "fil-PH", "Filipino (Philippines)" },
                    { "fo-FO", "Faroese (Faroe Islands)" },
                    { "fr-BE", "French (Belgium)" },
                    { "fr-CA", "French (Canada)" },
                    { "fr-CD", "French (Congo - Kinshasa)" },
                    { "fr-CH", "French (Switzerland)" },
                    { "fr-CI", "French (Côte d’Ivoire)" },
                    { "fr-CM", "French (Cameroon)" },
                    { "fr-FR", "French (France)" },
                    { "fr-HT", "French (Haiti)" },
                    { "fr-LU", "French (Luxembourg)" },
                    { "fr-MA", "French (Morocco)" },
                    { "fr-MC", "French (Monaco)" },
                    { "fr-ML", "French (Mali)" },
                    { "fr-RE", "French (Réunion)" },
                    { "es-UY", "Spanish (Uruguay)" },
                    { "en-PH", "English (Philippines)" },
                    { "es-US", "Spanish (United States)" },
                    { "es-PY", "Spanish (Paraguay)" },
                    { "en-TT", "English (Trinidad & Tobago)" },
                    { "en-US", "English (United States)" },
                    { "en-ZA", "English (South Africa)" },
                    { "en-ZW", "English (Zimbabwe)" },
                    { "es-419", "Spanish (Latin America)" },
                    { "es-AR", "Spanish (Argentina)" },
                    { "es-BO", "Spanish (Bolivia)" },
                    { "es-CL", "Spanish (Chile)" },
                    { "es-CO", "Spanish (Colombia)" },
                    { "es-CR", "Spanish (Costa Rica)" },
                    { "es-CU", "Spanish (Cuba)" },
                    { "es-DO", "Spanish (Dominican Republic)" },
                    { "es-EC", "Spanish (Ecuador)" },
                    { "es-ES", "Spanish (Spain)" },
                    { "es-GT", "Spanish (Guatemala)" },
                    { "es-HN", "Spanish (Honduras)" },
                    { "es-MX", "Spanish (Mexico)" },
                    { "es-NI", "Spanish (Nicaragua)" },
                    { "es-PA", "Spanish (Panama)" },
                    { "es-PE", "Spanish (Peru)" },
                    { "es-PR", "Spanish (Puerto Rico)" },
                    { "es-SV", "Spanish (El Salvador)" },
                    { "zu-ZA", "Zulu (South Africa)" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AllowedLanguages_LanguageIdentifier",
                table: "AllowedLanguages",
                column: "LanguageIdentifier");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AllowedLanguages");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Superusers");

            migrationBuilder.DropTable(
                name: "Languages");
        }
    }
}
