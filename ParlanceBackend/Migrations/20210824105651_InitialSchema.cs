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
                    Identifier = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Identifier);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    GitCloneUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Branch = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Superusers",
                columns: table => new
                {
                    UserId = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Superusers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "AllowedLanguages",
                columns: table => new
                {
                    UserId = table.Column<ulong>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LanguageIdentifier = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AllowedLanguages", x => x.UserId);
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
                values: new object[] { "af-ZA", "Afrikaans (South Africa)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ky-KG", "Kyrgyz (Kyrgyzstan)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "lb-LU", "Luxembourgish (Luxembourg)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "lo-LA", "Lao (Laos)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "lt-LT", "Lithuanian (Lithuania)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "lv-LV", "Latvian (Latvia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "mi-NZ", "Maori (New Zealand)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "mk-MK", "Macedonian (North Macedonia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ml-IN", "Malayalam (India)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "mn-MN", "Mongolian (Mongolia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "kok-IN", "Konkani (India)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "mr-IN", "Marathi (India)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ms-MY", "Malay (Malaysia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "mt-MT", "Maltese (Malta)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "my-MM", "Burmese (Myanmar [Burma])" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "nb-NO", "Norwegian Bokmål (Norway)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ne-IN", "Nepali (India)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ne-NP", "Nepali (Nepal)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "nl-BE", "Dutch (Belgium)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "nl-NL", "Dutch (Netherlands)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "nn-NO", "Norwegian Nynorsk (Norway)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ms-BN", "Malay (Brunei)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ko-KR", "Korean (South Korea)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "kn-IN", "Kannada (India)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "km-KH", "Khmer (Cambodia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "gl-ES", "Galician (Spain)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "gsw-FR", "Swiss German (France)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "gu-IN", "Gujarati (India)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "haw-US", "Hawaiian (United States)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "he-IL", "Hebrew (Israel)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "hi-IN", "Hindi (India)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "hr-BA", "Croatian (Bosnia & Herzegovina)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "hr-HR", "Croatian (Croatia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "hsb-DE", "Upper Sorbian (Germany)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "hu-HU", "Hungarian (Hungary)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "hy-AM", "Armenian (Armenia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "id-ID", "Indonesian (Indonesia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ig-NG", "Igbo (Nigeria)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ii-CN", "Sichuan Yi (China)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "is-IS", "Icelandic (Iceland)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "it-CH", "Italian (Switzerland)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "it-IT", "Italian (Italy)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ja-JP", "Japanese (Japan)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ka-GE", "Georgian (Georgia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "kk-KZ", "Kazakh (Kazakhstan)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "kl-GL", "Kalaallisut (Greenland)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "om-ET", "Oromo (Ethiopia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "or-IN", "Odia (India)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "pa-PK", "Punjabi (Arabic, Pakistan)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "pl-PL", "Polish (Poland)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "sv-FI", "Swedish (Finland)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "sv-SE", "Swedish (Sweden)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "sw-KE", "Swahili (Kenya)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ta-IN", "Tamil (India)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ta-LK", "Tamil (Sri Lanka)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "te-IN", "Telugu (India)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "th-TH", "Thai (Thailand)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ti-ER", "Tigrinya (Eritrea)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ti-ET", "Tigrinya (Ethiopia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "tk-TM", "Turkmen (Turkmenistan)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "tr-TR", "Turkish (Turkey)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "tt-RU", "Tatar (Russia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ug-CN", "Uyghur (China)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "uk-UA", "Ukrainian (Ukraine)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ur-IN", "Urdu (India)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ur-PK", "Urdu (Pakistan)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "uz-UZ", "Uzbek (Cyrillic, Uzbekistan)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "vi-VN", "Vietnamese (Vietnam)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "wo-SN", "Wolof (Senegal)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "xh-ZA", "Xhosa (South Africa)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "yi-001", "Yiddish (World)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "sr-RS", "Serbian (Cyrillic, Serbia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "gd-GB", "Scottish Gaelic (United Kingdom)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "sr-ME", "Serbian (Cyrillic, Montenegro)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "sq-AL", "Albanian (Albania)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ps-AF", "Pashto (Afghanistan)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "pt-BR", "Portuguese (Brazil)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "pt-PT", "Portuguese (Portugal)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "rm-CH", "Romansh (Switzerland)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ro-MD", "Romanian (Moldova)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ro-RO", "Romanian (Romania)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ru-MD", "Russian (Moldova)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ru-RU", "Russian (Russia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "rw-RW", "Kinyarwanda (Rwanda)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "sa-IN", "Sanskrit (India)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "sah-RU", "Yakut (Russia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "sd-PK", "Sindhi (Arabic, Pakistan)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "sd-IN", "Sindhi (Devanagari, India)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "se-FI", "Northern Sami (Finland)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "se-NO", "Northern Sami (Norway)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "se-SE", "Northern Sami (Sweden)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "si-LK", "Sinhala (Sri Lanka)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "sk-SK", "Slovak (Slovakia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "sl-SI", "Slovenian (Slovenia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "smn-FI", "Inari Sami (Finland)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "so-SO", "Somali (Somalia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "sr-BA", "Serbian (Cyrillic, Bosnia & Herzegovina)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ga-IE", "Irish (Ireland)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "fy-NL", "Western Frisian (Netherlands)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "fr-SN", "French (Senegal)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "bs-BA", "Bosnian (Cyrillic, Bosnia & Herzegovina)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ca-ES", "Catalan (Spain)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "cs-CZ", "Czech (Czechia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "cy-GB", "Welsh (United Kingdom)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "da-DK", "Danish (Denmark)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "de-AT", "German (Austria)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "de-CH", "German (Switzerland)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "de-DE", "German (Germany)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "de-LI", "German (Liechtenstein)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "de-LU", "German (Luxembourg)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "dsb-DE", "Lower Sorbian (Germany)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "dz-BT", "Dzongkha (Bhutan)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "el-GR", "Greek (Greece)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "en-AU", "English (Australia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "en-BZ", "English (Belize)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "en-CA", "English (Canada)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "en-GB", "English (United Kingdom)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "en-HK", "English (Hong Kong SAR China)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "en-IE", "English (Ireland)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "en-IN", "English (India)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "en-JM", "English (Jamaica)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "br-FR", "Breton (France)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "en-MY", "English (Malaysia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "bo-CN", "Tibetan (China)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "bn-BD", "Bengali (Bangladesh)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "am-ET", "Amharic (Ethiopia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ar-AE", "Arabic (United Arab Emirates)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ar-BH", "Arabic (Bahrain)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ar-DZ", "Arabic (Algeria)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ar-EG", "Arabic (Egypt)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ar-IQ", "Arabic (Iraq)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ar-JO", "Arabic (Jordan)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ar-KW", "Arabic (Kuwait)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ar-LB", "Arabic (Lebanon)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ar-LY", "Arabic (Libya)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ar-MA", "Arabic (Morocco)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ar-OM", "Arabic (Oman)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ar-QA", "Arabic (Qatar)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ar-SA", "Arabic (Saudi Arabia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ar-SY", "Arabic (Syria)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ar-TN", "Arabic (Tunisia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ar-YE", "Arabic (Yemen)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "as-IN", "Assamese (India)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "az-AZ", "Azerbaijani (Cyrillic, Azerbaijan)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "be-BY", "Belarusian (Belarus)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "bg-BG", "Bulgarian (Bulgaria)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "bn-IN", "Bengali (India)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "yo-NG", "Yoruba (Nigeria)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "en-NZ", "English (New Zealand)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "en-SG", "English (Singapore)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-VE", "Spanish (Venezuela)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "et-EE", "Estonian (Estonia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "eu-ES", "Basque (Spain)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "fa-IR", "Persian (Iran)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "ff-SN", "Fulah (Latin, Senegal)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "fi-FI", "Finnish (Finland)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "fil-PH", "Filipino (Philippines)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "fo-FO", "Faroese (Faroe Islands)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "fr-BE", "French (Belgium)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "fr-CA", "French (Canada)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "fr-CD", "French (Congo - Kinshasa)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "fr-CH", "French (Switzerland)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "fr-CI", "French (Côte d’Ivoire)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "fr-CM", "French (Cameroon)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "fr-FR", "French (France)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "fr-HT", "French (Haiti)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "fr-LU", "French (Luxembourg)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "fr-MA", "French (Morocco)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "fr-MC", "French (Monaco)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "fr-ML", "French (Mali)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "fr-RE", "French (Réunion)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-UY", "Spanish (Uruguay)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "en-PH", "English (Philippines)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-US", "Spanish (United States)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-PY", "Spanish (Paraguay)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "en-TT", "English (Trinidad & Tobago)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "en-US", "English (United States)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "en-ZA", "English (South Africa)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "en-ZW", "English (Zimbabwe)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-419", "Spanish (Latin America)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-AR", "Spanish (Argentina)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-BO", "Spanish (Bolivia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-CL", "Spanish (Chile)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-CO", "Spanish (Colombia)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-CR", "Spanish (Costa Rica)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-CU", "Spanish (Cuba)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-DO", "Spanish (Dominican Republic)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-EC", "Spanish (Ecuador)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-ES", "Spanish (Spain)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-GT", "Spanish (Guatemala)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-HN", "Spanish (Honduras)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-MX", "Spanish (Mexico)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-NI", "Spanish (Nicaragua)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-PA", "Spanish (Panama)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-PE", "Spanish (Peru)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-PR", "Spanish (Puerto Rico)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "es-SV", "Spanish (El Salvador)" });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Identifier", "Name" },
                values: new object[] { "zu-ZA", "Zulu (South Africa)" });

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
