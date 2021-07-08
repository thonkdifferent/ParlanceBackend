import React from 'react';
import {
    BrowserRouter as Router,
    Switch,
    Route,
    Link,
    withRouter,
    Prompt
} from "react-router-dom";
import hotkeys from "react-keyboard-shortcuts";

import ContextSearch from './ContextSearch';

import PoManager from "./PoManager";
import Styles from "./index.module.css";
import Context from "./Context";
import TranslationArea from './TranslationArea';
import TranslationItem from './TranslationItem';

class TranslationEditor extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            po: null,
            selection: null,
            searchQuery: "",
            flags: {
                unfinishedOnly: false
            }
        }
    }

    async componentDidMount() {
        //Grab the po file from the server. Here we'll hardcode it.
        let fileContents = await ((async() => `msgid ""
        msgstr ""
        "Project-Id-Version: Dutch (theBeat)\n"
        "Report-Msgid-Bugs-To: \n"
        "POT-Creation-Date: 2021-07-08 11:09+0000\n"
        "PO-Revision-Date: YEAR-MO-DA HO:MI+ZONE\n"
        "Last-Translator: FULL NAME <EMAIL@ADDRESS>\n"
        "Language-Team: Dutch <https://translate.vicr123.com/projects/thebeat/"
        "thebeat-3-0/nl/>\n"
        "Language: nl\n"
        "MIME-Version: 1.0\n"
        "Content-Type: text/plain; charset=UTF-8\n"
        "Content-Transfer-Encoding: 8bit\n"
        "Plural-Forms: nplurals=2; plural=n != 1;\n"
        "X-Generator: Weblate 4.1.1\n"
        
        #: ../artistsalbumswidget.ui:65,
        #: ../artistsalbumswidget.ui:188
        msgctxt "ArtistsAlbumsWidget"
        msgid "Tracks in Library"
        msgstr "Nummers in Bibliotheek"
        
        #: ../artistsalbumswidget.ui:230
        msgctxt "ArtistsAlbumsWidget"
        msgid "Enqueue All"
        msgstr "Alles in Wachtrij Zetten"
        
        #: ../artistsalbumswidget.cpp:67
        msgctxt "ArtistsAlbumsWidget"
        msgid "Albums in Library"
        msgstr "Albums in Bibliotheek"
        
        #: ../artistsalbumswidget.cpp:67
        msgctxt "ArtistsAlbumsWidget"
        msgid "Artists in Library"
        msgstr "Artiesten in Bibliotheek"
        
        #: ../artistsalbumswidget.cpp:153
        msgctxt "ArtistsAlbumsWidget"
        msgid "Tracks in %1"
        msgstr "Nummers in %1"
        
        #: ../artistsalbumswidget.cpp:153
        msgctxt "ArtistsAlbumsWidget"
        msgid "Tracks by %1"
        msgstr "Nummers van %1"
        
        #: ../controlstrip.ui:102
        msgctxt "ControlStrip"
        msgid "Title"
        msgstr "Titel"
        
        #: ../controlstrip.ui:109
        msgctxt "ControlStrip"
        msgid "Metadata"
        msgstr "Metadata"
        
        #: ../libraryerrorpopover.ui:35
        msgctxt "LibraryErrorPopover"
        msgid "Hmm"
        msgstr "Hmm"
        
        #: ../libraryerrorpopover.ui:134
        msgctxt "LibraryErrorPopover"
        msgid "ACTIONS"
        msgstr "ACTIES"
        
        #: ../libraryerrorpopover.ui:143
        msgctxt "LibraryErrorPopover"
        msgid "Locate File"
        msgstr "Bestand Zoeken"
        
        #: ../libraryerrorpopover.ui:150
        msgctxt "LibraryErrorPopover"
        msgid "Remove from Library"
        msgstr "Verwijderen uit Bibliotheek"
        
        #: ../libraryerrorpopover.cpp:39
        msgctxt "LibraryErrorPopover"
        msgid ""
        "Looks like the file has gone into hiding. If you know where it is, let us "
        "know so we can play the track."
        msgstr ""
        "Het lijkt erop dat het bestand zoek is geraakt. Als jij wel weet waar het "
        "is, laat het ons dan weten zodat we het nummer kunnen afspelen."
        
        #: ../library/librarymodel.cpp:174
        msgctxt "LibraryItemDelegate"
        msgid "by %1"
        msgstr "door %1"
        
        #: ../library/librarymodel.cpp:175
        msgctxt "LibraryItemDelegate"
        msgid "on %1"
        msgstr "op %1"
        
        #: ../library/librarymodel.cpp:177
        msgctxt "LibraryItemDelegate"
        msgid "Track"
        msgstr "Nummer"
        
        #: ../library/librarylistview.cpp:28
        msgctxt "LibraryListView"
        msgid "Add to Playlist"
        msgstr "Toevoegen aan Afspeellijst"
        
        #: ../library/librarylistview.cpp:43,
        #: ../library/librarylistview.cpp:45
        msgctxt "LibraryListView"
        msgid "Remove from Library"
        msgstr "Verwijderen uit Bibliotheek"
        
        #: ../library/librarylistview.cpp:127
        msgctxt "LibraryListView"
        msgid "For %1"
        msgstr "Voor %1"
        
        #: ../library/librarylistview.cpp:132
        msgctxt "LibraryListView"
        msgid "For %n items"
        msgid_plural "For %n items"
        msgstr[0] "Voor %n ding"
        msgstr[1] "Voor %n dingen"
        
        #: ../mainwindow.ui:14
        msgctxt "MainWindow"
        msgid "theBeat"
        msgstr "theBeat"
        
        #: ../mainwindow.ui:90
        msgctxt "MainWindow"
        msgid "Tracks"
        msgstr "Nummers"
        
        #: ../mainwindow.ui:116
        msgctxt "MainWindow"
        msgid "Artists"
        msgstr "Artiesten"
        
        #: ../mainwindow.ui:139
        msgctxt "MainWindow"
        msgid "Albums"
        msgstr "Albums"
        
        #: ../mainwindow.ui:162
        msgctxt "MainWindow"
        msgid "Playlists"
        msgstr "Afspeellijsten"
        
        #: ../mainwindow.ui:185
        msgctxt "MainWindow"
        msgid "Other Sources"
        msgstr "Andere Bronnen"
        
        #: ../mainwindow.ui:276
        msgctxt "MainWindow"
        msgid "Queue"
        msgstr "Wachtrij"
        
        #: ../mainwindow.ui:346
        msgctxt "MainWindow"
        msgid "Nothing Here!"
        msgstr "Hier is niks!"
        
        #: ../mainwindow.ui:436
        msgctxt "MainWindow"
        msgid "File Bug"
        msgstr "Een Bug Rapporteren"
        
        #: ../mainwindow.ui:445
        msgctxt "MainWindow"
        msgid "Sources"
        msgstr "Bronnen"
        
        #: ../mainwindow.ui:454
        msgctxt "MainWindow"
        msgid "About"
        msgstr "Over"
        
        #: ../mainwindow.ui:463
        msgctxt "MainWindow"
        msgid "Exit"
        msgstr "Stop"
        
        #: ../mainwindow.ui:466
        msgctxt "MainWindow"
        msgid "Ctrl+Q"
        msgstr "Ctrl+Q"
        
        #: ../mainwindow.ui:478
        msgctxt "MainWindow"
        msgid "Open File"
        msgstr "Bestand Openen"
        
        #: ../mainwindow.ui:481
        msgctxt "MainWindow"
        msgid "Ctrl+O"
        msgstr "Ctrl+O"
        
        #: ../mainwindow.ui:486,
        #: ../mainwindow.cpp:440
        msgctxt "MainWindow"
        msgid "Open URL"
        msgstr "URL Openen"
        
        #: ../mainwindow.ui:495,
        #: ../mainwindow.cpp:211
        msgctxt "MainWindow"
        msgid "Skip Back"
        msgstr "Terug Springen"
        
        #: ../mainwindow.ui:498
        msgctxt "MainWindow"
        msgid "Shift+Left"
        msgstr "Shift+Links"
        
        #: ../mainwindow.ui:510
        msgctxt "MainWindow"
        msgid "Shift+Right"
        msgstr "Shift+Rechts"
        
        #: ../mainwindow.ui:531
        msgctxt "MainWindow"
        msgid "Add to Library"
        msgstr "Toevoegen aan Bibliotheek"
        
        #: ../mainwindow.ui:540
        msgctxt "MainWindow"
        msgid "Settings"
        msgstr "Instellingen"
        
        #: ../mainwindow.ui:543
        msgctxt "MainWindow"
        msgid "Ctrl+,"
        msgstr "Ctrl+,"
        
        #: ../mainwindow.ui:507
        msgctxt "MainWindow"
        msgid "Skip Forward"
        msgstr "Vooruit Springen"
        
        #: ../mainwindow.ui:356
        msgctxt "MainWindow"
        msgid "Select a track or drop something here!"
        msgstr "Selecteer een nummer of sleep hier iets heen!"
        
        #: ../mainwindow.ui:519,
        #: ../mainwindow.cpp:219,
        #: ../mainwindow.cpp:233
        msgctxt "MainWindow"
        msgid "Play"
        msgstr "Afspelen"
        
        #: ../mainwindow.ui:522
        msgctxt "MainWindow"
        msgid "Space"
        msgstr "Spatie"
        
        #: ../mainwindow.cpp:228
        msgctxt "MainWindow"
        msgid "Pause"
        msgstr "Pauzeren"
        
        #: ../mainwindow.cpp:240
        msgctxt "MainWindow"
        msgid "Skip Next"
        msgstr "Overslaan"
        
        #: ../mainwindow.cpp:412
        msgctxt "MainWindow"
        msgid "For %1"
        msgstr "Voor %1"
        
        #: ../mainwindow.cpp:414
        msgctxt "MainWindow"
        msgid "For %n items"
        msgid_plural "For %n items"
        msgstr[0] "Voor %n ding"
        msgstr[1] "Voor %n dingen"
        
        #: ../mainwindow.cpp:416
        msgctxt "MainWindow"
        msgid "Remove from Queue"
        msgstr "Verwijderen uit Wachtrij"
        
        #: ../mainwindow.cpp:429
        msgctxt "MainWindow"
        msgid "For Queue"
        msgstr "Voor Wachtrij"
        
        #: ../mainwindow.cpp:430
        msgctxt "MainWindow"
        msgid "Clear Queue"
        msgstr "Wachtrij Legen"
        
        #: ../othersourceswidget.ui:103
        msgctxt "OtherSourcesWidget"
        msgid "No other sources available"
        msgstr "Geen andere bronnen beschikbaar"
        
        #: ../othersourceswidget.ui:113
        msgctxt "OtherSourcesWidget"
        msgid "There's nothing else to play right now"
        msgstr "Er is niets anders om af te spelen"
        
        #: ../settingsdialog.ui:14,
        #: ../settingsdialog.ui:53
        msgctxt "SettingsDialog"
        msgid "Settings"
        msgstr "Instellingen"
        
        #: ../settingsdialog.ui:64,
        #: ../settingsdialog.ui:120
        msgctxt "SettingsDialog"
        msgid "Library"
        msgstr "Bibliotheek"
        
        #: ../settingsdialog.ui:74,
        #: ../settingsdialog.ui:424
        msgctxt "SettingsDialog"
        msgid "Notifications"
        msgstr "Meldingen"
        
        #: ../settingsdialog.ui:180
        msgctxt "SettingsDialog"
        msgid "RESET"
        msgstr "RESETTEN"
        
        #: ../settingsdialog.ui:187
        msgctxt "SettingsDialog"
        msgid ""
        "Reset the library back to defaults. This will clear all tracks that have "
        "been added to theBeat, and theBeat will rescan your Music folder upon "
        "startup.\n"
        "\n"
        "theBeat will restart once the reset is complete. This action is irreversible."
        msgstr ""
        "Herstelt de bibliotheek naar standaardwaarden. Dit zal alle nummers die aan "
        "theBeat zijn toegevoegd verwijderen, en theBeat zal je Muziek-map opnieuw "
        "scannen als het opstart.\n"
        "\n"
        "theBeat zal opnieuw starten zodra de reset klaar is. Deze actie is niet "
        "terug te draaien."
        
        #: ../settingsdialog.ui:201
        msgctxt "SettingsDialog"
        msgid "Reset theBeat Library"
        msgstr "theBeat-bibliotheek Resetten"
        
        #: ../settingsdialog.ui:481
        msgctxt "SettingsDialog"
        msgid "Send a notification when the track changes"
        msgstr "Een melding sturen als het nummer verandert"
        
        #: ../trackswidget.ui:65
        msgctxt "TracksWidget"
        msgid "Tracks in Library"
        msgstr "Nummers in Bibliotheek"
        
        #: ../trackswidget.ui:74
        msgctxt "TracksWidget"
        msgid "Enqueue All"
        msgstr "Alles in de Wachtrij Zetten"
        
        #: ../trackswidget.ui:119
        msgctxt "TracksWidget"
        msgid "Search"
        msgstr "Zoeken"
        
        #: ../trackswidget.ui:176
        msgctxt "TracksWidget"
        msgid "Processing Library"
        msgstr "Bibliotheek aan het Verwerken"
        
        #: ../trackswidget.ui:183
        msgctxt "TracksWidget"
        msgid ""
        "Depending on the number of tracks in your library, this could take a while. "
        "This should only happen the first time you start theBeat."
        msgstr ""
        "Afhankelijk van het aantal nummers in je bibliotheek, kan dit even duren. "
        "Dit hoort alleen de eerste keer dat je theBeat start te gebeuren."
        
        #: ../userplaylistswidget.ui:62
        msgctxt "UserPlaylistsWidget"
        msgid "Playlists"
        msgstr "Afspeellijsten"
        
        #: ../userplaylistswidget.ui:71
        msgctxt "UserPlaylistsWidget"
        msgid "Create Playlist"
        msgstr "Afspeellijst Maken"
        
        #: ../userplaylistswidget.ui:222
        msgctxt "UserPlaylistsWidget"
        msgid "Tracks in Library"
        msgstr "Nummers in Bibliotheek"
        
        #: ../userplaylistswidget.ui:250
        msgctxt "UserPlaylistsWidget"
        msgid "Enqueue All"
        msgstr "Alles in de Wachtrij Zetten"
        
        #: ../userplaylistswidget.cpp:120,
        #: ../userplaylistswidget.cpp:174
        msgctxt "UserPlaylistsWidget"
        msgid "Tracks in %1"
        msgstr "Nummers in %1"
        
        #: ../main.cpp:55
        msgctxt "main"
        msgid "Audio Player"
        msgstr "Audiospeler"
        
        #: ../currenttrackpopover.cpp:130
        msgctxt "CurrentTrackPopover"
        msgid "Album"
        msgstr "Album"
        
        #: ../currenttrackpopover.cpp:131
        msgctxt "CurrentTrackPopover"
        msgid "Name"
        msgstr "Naam"
        
        #: ../currenttrackpopover.cpp:136,
        #: ../currenttrackpopover.cpp:138,
        #: ../currenttrackpopover.cpp:141
        msgctxt "CurrentTrackPopover"
        msgid "Track"
        msgstr "Nummer"
        
        #: ../currenttrackpopover.cpp:136
        msgctxt ""
        "CurrentTrackPopover\n"
        "Track 1 of 12"
        msgid "%1 of %2"
        msgstr "%1 van %2"
        
        #: ../currenttrackpopover.cpp:142
        msgctxt "CurrentTrackPopover"
        msgid "Year"
        msgstr "Jaar"
        
        #: ../userplaylistswidget.ui:316
        msgctxt "UserPlaylistsWidget"
        msgid "Burn"
        msgstr "Branden"
        
        #: ../artistsalbumswidget.ui:241
        msgctxt "ArtistsAlbumsWidget"
        msgid "Play All"
        msgstr "Alles Spelen"
        
        #: ../artistsalbumswidget.ui:276
        msgctxt "ArtistsAlbumsWidget"
        msgid "Burn"
        msgstr "Branden"
        
        #: ../artistsalbumswidget.cpp:162
        msgctxt "ArtistsAlbumsWidget"
        msgid "%n tracks"
        msgid_plural "%n tracks"
        msgstr[0] "%n nummer"
        msgstr[1] "%n nummers"
        
        #: ../common.cpp:31
        msgctxt "Common"
        msgid "Select Device"
        msgstr "Apparaat Selecteren"
        
        #: ../playlistmodel.cpp:259
        msgctxt "PlaylistDelegate"
        msgid "Track"
        msgstr "Nummer"
        
        #: ../playlistmodel.cpp:282
        msgctxt "PlaylistDelegate"
        msgid "Album"
        msgstr "Album"
        
        #: ../settingsdialog.ui:69,
        #: ../settingsdialog.ui:248
        msgctxt "SettingsDialog"
        msgid "Appearance"
        msgstr "Uiterlijk"
        
        #: ../settingsdialog.ui:311
        msgctxt "SettingsDialog"
        msgid "TITLEBAR"
        msgstr "TITELBALK"
        
        #: ../settingsdialog.ui:318
        msgctxt "SettingsDialog"
        msgid "Use System Titlebars"
        msgstr "Titelbalken van het Systeem Gebruiken"
        
        #: ../library/libraryenumeratedirectoryjob.cpp:153
        msgctxt "LibraryEnumerateDirectoryJob"
        msgid "Folder Added"
        msgstr "Map Toegevoegd"
        
        #: ../library/libraryenumeratedirectoryjob.cpp:154
        msgctxt "LibraryEnumerateDirectoryJob"
        msgid "%n tracks added/updated"
        msgid_plural "%n tracks added/updated"
        msgstr[0] "%n nummer toegevoegd/bijgewerkt"
        msgstr[1] "%n nummers toegevoegd/bijgewerkt"
        
        #: ../library/libraryenumeratedirectoryjobwidget.ui:26
        msgctxt "LibraryEnumerateDirectoryJobWidget"
        msgid "DISCOVERING AUDIO TRACKS"
        msgstr "GELUIDSFRAGMENTEN AAN HET ONTDEKKEN"
        
        #: ../library/librarylistview.cpp:33,
        #: ../library/librarylistview.cpp:44
        msgctxt "LibraryListView"
        msgid "Are you sure?"
        msgstr "Weet je het zeker?"
        
        #: ../library/librarylistview.cpp:32,
        #: ../library/librarylistview.cpp:34
        msgctxt "LibraryListView"
        msgid "Remove from Playlist"
        msgstr "Verwijderen uit Afspeellijst"
        
        #: ../library/librarylistview.cpp:110
        msgctxt "LibraryListView"
        msgid "New Playlist"
        msgstr "Nieuwe Afspeellijst"
        
        #: ../library/librarylistview.cpp:112
        msgctxt "LibraryListView"
        msgid "Playlist Name"
        msgstr "Naam van de Afspeellijst"
        
        #: ../userplaylistswidget.cpp:149,
        #: ../userplaylistswidget.cpp:151,
        #: ../userplaylistswidget.cpp:161,
        #: ../userplaylistswidget.cpp:163
        msgctxt "UserPlaylistsWidget"
        msgid "Remove"
        msgstr "Verwijderen"
        
        #: ../userplaylistswidget.cpp:150,
        #: ../userplaylistswidget.cpp:162
        msgctxt "UserPlaylistsWidget"
        msgid "Are you sure?"
        msgstr "Weet je het zeker?"
        
        #: ../userplaylistswidget.cpp:114
        msgctxt "UserPlaylistsWidget"
        msgid "For %1"
        msgstr "Voor %1"
        
        #: ../userplaylistswidget.cpp:115,
        #: ../userplaylistswidget.cpp:117
        msgctxt "UserPlaylistsWidget"
        msgid "Rename"
        msgstr "Hernoemen"
        
        #: ../userplaylistswidget.cpp:157
        msgctxt "UserPlaylistsWidget"
        msgid "For %n playlists"
        msgid_plural "For %n playlists"
        msgstr[0] "Voor %n afspeellijst"
        msgstr[1] "Voor %n afspeellijsten"
        
        #: ../userplaylistswidget.ui:261
        msgctxt "UserPlaylistsWidget"
        msgid "Play All"
        msgstr "Alles Spelen"
        
        #: ../main.cpp:98
        msgctxt "main"
        msgid "file"
        msgstr "bestand"
        
        #: ../main.cpp:98
        msgctxt "main"
        msgid "File to open"
        msgstr "Bestand om te openen"
        
        #: ../mainwindow.ui:555
        msgctxt "MainWindow"
        msgid "theBeat Help"
        msgstr "theBeat-hulp"
        
        #: ../mainwindow.ui:558
        msgctxt "MainWindow"
        msgid "F1"
        msgstr "F1"
        
        #: ../controlstrip.cpp:82
        msgctxt "ControlStrip"
        msgid "Repeat Options"
        msgstr "Herhalingsopties"
        
        #: ../controlstrip.cpp:83
        msgctxt "ControlStrip"
        msgid "Repeat Play Queue"
        msgstr "Wachtrij Herhalen"
        
        #: ../userplaylistswidget.cpp:123
        msgctxt "UserPlaylistsWidget"
        msgid "Export"
        msgstr "Exporteren"
        
        #: ../settingsdialog.ui:364
        msgctxt "SettingsDialog"
        msgid "COLOURS"
        msgstr "KLEUREN"
        
        #: ../settingsdialog.ui:371
        msgctxt "SettingsDialog"
        msgid "Light"
        msgstr "Licht"
        
        #: ../settingsdialog.ui:378
        msgctxt "SettingsDialog"
        msgid "Dark"
        msgstr "Donker"
        
        #: ../controlstrip.cpp:100
        msgctxt "ControlStrip"
        msgid "Playback Options"
        msgstr ""
        
        #: ../controlstrip.cpp:101
        msgctxt "ControlStrip"
        msgid "Pause after current track"
        msgstr ""
        
        #: ../visualisations/scopevisualisation.cpp:38
        msgctxt "ScopeVisualisation"
        msgid "Scope"
        msgstr ""
        
        #: ../artistsalbumswidget.ui:252
        msgctxt "ArtistsAlbumsWidget"
        msgid "Shuffle All"
        msgstr ""
        
        #: ../mainwindow.ui:408
        msgctxt "MainWindow"
        msgid "File"
        msgstr ""
        
        #: ../mainwindow.ui:421
        msgctxt "MainWindow"
        msgid "Playback"
        msgstr ""
        
        #: ../userplaylistswidget.ui:272
        msgctxt "UserPlaylistsWidget"
        msgid "Shuffle All"
        msgstr ""
        
        #: ../mainwindow.cpp:440
        msgctxt "MainWindow"
        msgid "Enter the URL you'd like to open"
        msgstr ""
        
        #: ../mainwindow.cpp:445
        msgctxt "MainWindow"
        msgid "Can't open that URL"
        msgstr ""
        
        #: ../mainwindow.cpp:445
        msgctxt "MainWindow"
        msgid "Sorry, that URL isn't supported by theBeat."
        msgstr ""
        
        #: ../userplaylistswidget.cpp:73
        #, fuzzy
        msgctxt "UserPlaylistsWidget"
        msgid "New Playlist"
        msgstr "Nieuwe Afspeellijst"
        
        #: ../userplaylistswidget.cpp:73,
        #: ../userplaylistswidget.cpp:117
        msgctxt "UserPlaylistsWidget"
        msgid "What name do you want to give to this playlist?"
        msgstr ""
        `)());

        let po = new PoManager(fileContents, this.props.match.params.language);
        po.on("translationsChanged", () => {
            this.forceUpdate()
        });

        this.setState({
            po
        });
    }

    hot_keys = {
        'ctrl+s': {
            priority: 1,
            handler: (e) => {
                alert("control S")
                e.preventDefault();
            }
        },
        'alt+down': {
            priority: 1,
            handler: (e) => {
                e.preventDefault();
                this.setState(oldState => {
                    return {
                        selection: this.getNextItem()
                    }
                });
            }
        },
        'alt+up': {
            priority: 1,
            handler: (e) => {
                e.preventDefault();
                this.setState(oldState => {
                    return {
                        selection: this.getPreviousItem()
                    }
                });
            }
        }
    }
    
    select(selection) {
        this.setState({
            selection
        });
    }

    search(query) {
        this.setState({
            searchQuery: query
        })
    }

    setFlags(flags) {
        this.setState({
            flags
        })
    }

    getNextItem() {
        let nextSelection = this.state.selection;
        let firstSelection = this.state.selection;
        do {
            nextSelection = this.state.po.nextSelection(nextSelection?.context, nextSelection?.key);
            if (JSON.stringify(nextSelection) === JSON.stringify(firstSelection)) return firstSelection;
        } while (!TranslationItem.shouldRender(this.state.po, nextSelection.context, nextSelection.key, this.state.searchQuery, this.state.flags));

        return nextSelection;
    }

    getPreviousItem() {
        let previousSelection = this.state.selection;
        let firstSelection = this.state.selection;
        do {
            previousSelection = this.state.po.previousSelection(previousSelection?.context, previousSelection?.key);
            if (JSON.stringify(previousSelection) === JSON.stringify(firstSelection)) return firstSelection;
        } while (!TranslationItem.shouldRender(this.state.po, previousSelection.context, previousSelection.key, this.state.searchQuery, this.state.flags));

        return previousSelection;
    }

    render() {
        if (this.state.po) {
            if (this.state.po.hasError) {
                return "There was an error loading the translation file.";
            } else {
                return <div className={Styles.EditorRoot}>
                    <div className={Styles.ContextListWrapper}>
                        <ContextSearch searchQuery={this.state.searchQuery} onSearch={this.search.bind(this)} flags={this.state.flags} onSetFlags={this.setFlags.bind(this)} />
                        <div className={Styles.ContextList}>
                            {this.state.po.contexts().map(context => <Context key={context} searchQuery={this.state.searchQuery} flags={this.state.flags} context={context} poManager={this.state.po} selection={this.state.selection} onSelect={this.select.bind(this)} />)}
                        </div>
                    </div>
                    <TranslationArea selection={this.state.selection} poManager={this.state.po} />
                </div>
            }
        } else {
            return "Hang on...";
        }
    }
}

export default withRouter(hotkeys(TranslationEditor));