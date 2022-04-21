﻿using Super.Paula.Application.Auth.Exceptions;
using System.Collections.Generic;
using System.Linq;

namespace Super.Paula.Client.Localization
{
    public class LocalGermanTranslationHandler : LocalAbstractTranslationHandler
    {
        public override IDictionary<(string? Category, string Hash), TranslationInfo> Translations => _translations;

        private readonly IDictionary<(string? Category, string Hash), TranslationInfo> _translations
            = new Dictionary<(string?, string), TranslationInfo>
            {
                [("business-object-inspection-audits-audit", "0d7c240cba1cf038ff57e08f553a5b87e790fa6f")] = "Inspektionen",
                [("business-object-inspection-audits-audit", "1431f68f359f8699975be39dca08302c67d68d9e")] = "Nicht gesetzt",
                [("business-object-inspection-audits-audit", "4f5f32d8fd6737e69e2b6dd86c258cb0d90603ce")] = "Kommentieren",
                [("business-object-inspection-audits-audit", "5faa59d4bc3756040b8ce9e673c09f929e6ee9ba")] = "Ergebnis",
                [("business-object-inspection-audits-audit", "d3ce4618efaae8bf391d0768eaf2c4b834adfb9b")] = "Inspektion",
                [("business-object-inspection-audits-audit", "e6faf83624afed8ffb167ed9d4c2757ecb83bf49")] = "Zufriedenstellend",
                [("business-object-inspection-audits-audit", "8909bf2b511b41dd229fe3d05504b8386952fbaf")] = "Ungenügend",
                [("business-object-inspection-audits-audit-annotation", "0641bb14fd8edfc862e3a7958fab63a9c7df222e")] = "Auditergebnis",
                [("business-object-inspection-audits-audit-annotation", "2dacf65959849884a011f36f76a04eebea94c5ea")] = "Speichern",
                [("business-object-inspection-audits-audit-annotation", "9f81bd1f9c717e25b277119bebb6fda7961ae905")] = "Audit kommentieren",
                [("business-object-inspection-audits-audit-annotation", "c7874aaa0fab4eeff4f2db2ec37d5f5a2dea859a")] = "Anzeigename",
                [("business-object-inspection-audits-audit-annotation", "de3b78b64190965365f72568ffbe049d4ba32c22")] = "Kommentar",
                [("business-object-inspection-audits-audit-history", "1569d15bf1046333fa3a702fad92702489c1b072")] = "Auditzeitpunkt",
                [("business-object-inspection-audits-audit-history", "44913866dafb9ecbfe606b3b94734564978a688b")] = "Ungenügend anzeigen",
                [("business-object-inspection-audits-audit-history", "50f94286ba30706a19070d3ec0a0c8d34d6cf6eb")] = "Zurück",
                [("business-object-inspection-audits-audit-history", "5faa59d4bc3756040b8ce9e673c09f929e6ee9ba")] = "Ergebnis",
                [("business-object-inspection-audits-audit-history", "8222c902ae0723e8bb352bb17ed76809e4688165")] = "Inspektor",
                [("business-object-inspection-audits-audit-history", "876f874dc1ca495d673968fbff202ef0ac29bce0")] = "Geschäftsobjekt",
                [("business-object-inspection-audits-audit-history", "9638ab38272329798b24d69c048d6aa8138f2627")] = "Zufriedenstellend anzeigen",
                [("business-object-inspection-audits-audit-history", "bc981983e7f547dc62e19a1e383acfe00782a6d5")] = "Weiter",
                [("business-object-inspection-audits-audit-history", "c67a4d8c2ec2266e9800be69982583356c51d125")] = "Inspektionsaudits",
                [("business-object-inspection-audits-audit-history", "d3ce4618efaae8bf391d0768eaf2c4b834adfb9b")] = "Inspektion",
                [("business-object-inspection-audits-audit-history", "e6faf83624afed8ffb167ed9d4c2757ecb83bf49")] = "Zufriedenstellend",
                [("business-object-inspection-audits-audit-history", "ecec50d90865c759cc9ddbc1f9df28040c29cc13")] = "Durchgefallende anzeigen",
                [("business-object-inspection-audits-audit-history-for-business-objects", "44913866dafb9ecbfe606b3b94734564978a688b")] = "Ungenügend anzeigen",
                [("business-object-inspection-audits-audit-history-for-business-objects", "50f94286ba30706a19070d3ec0a0c8d34d6cf6eb")] = "Zurück",
                [("business-object-inspection-audits-audit-history-for-business-objects", "9638ab38272329798b24d69c048d6aa8138f2627")] = "Zufriedenstellend anzeigen",
                [("business-object-inspection-audits-audit-history-for-business-objects", "bc981983e7f547dc62e19a1e383acfe00782a6d5")] = "Weiter",
                [("business-object-inspection-audits-audit-history-for-business-objects", "c67a4d8c2ec2266e9800be69982583356c51d125")] = "Inspektionsaudits",
                [("business-object-inspection-audits-audit-history-for-business-objects", "ecec50d90865c759cc9ddbc1f9df28040c29cc13")] = "Durchgefallende anzeigen",
                [("business-object-inspection-audits-auditing", "6cd43a542448c08114c503781c0b9417253001e2")] = "Geplanter Auditzeitpunkt",
                [("business-object-inspection-audits-auditing", "876f874dc1ca495d673968fbff202ef0ac29bce0")] = "Geschäftsobjekt",
                [("business-object-inspection-audits-auditing", "b5d8af1730d32bfc7cd2a2d466b253948d799330")] = "Einzigartiger Name",
                [("business-object-inspection-audits-auditing", "cb5cc8e2ad78882026ea4054f5bcf86b043d26db")] = "Auditing",
                [("business-object-inspection-audits-auditing", "fa1703dd78a038dfd3482154532676941fd0de86")] = "Audit",
                [("business-object-inspection-response-toggle", "aec693a7533fd48f0fe10f64557451fa7fbffd01")] = "{0} (nächstes Audit)",
                [("business-object-inspection-response-toggle", "c8c85a7927ce374d611af3c8ae22cd3c8574bae6")] = "{0} (letztes Audit)",
                [("business-object-inspections-view", "0a088b3115056ef6acd3ab4b406c823e1724e216")] = "Überspringen",
                [("business-object-inspections-view", "0a8adac9d6d5f4d14ee887d196cd8618f0f3391a")] = "Planen",
                [("business-object-inspections-view", "0d7c240cba1cf038ff57e08f553a5b87e790fa6f")] = "Inspektionen",
                [("business-object-inspections-view", "3bf8e42862e41f78854b0d6bf4c4016f61713763")] = "Planungsmodus",
                [("business-object-inspections-view", "51223cfaf427017c1e3f8e4d3036506583d004b7")] = "Übersprungmodus",
                [("business-object-inspections-view", "896bfd3a9add773bd13cafb0ece2fb5c2d6d3694")] = "Nicht zugewiesen",
                [("business-object-inspections-view", "a922019ebcf73648d83879391cdfda5042f1ac87")] = "Zuweisungsmodusmodus",
                [("business-object-inspections-view", "b5d8af1730d32bfc7cd2a2d466b253948d799330")] = "Einzigartiger Name",
                [("business-object-inspections-view", "c7874aaa0fab4eeff4f2db2ec37d5f5a2dea859a")] = "Anzeigename",
                [("business-objects-create", "2dacf65959849884a011f36f76a04eebea94c5ea")] = "Speichern",
                [("business-objects-create", "8222c902ae0723e8bb352bb17ed76809e4688165")] = "Inspector",
                [("business-objects-create", "8f4feeeb6cbeef8d236a210ffc34ef9f42d44eb4")] = "Geschäftsobjekt anlegen",
                [("business-objects-create", "b5d8af1730d32bfc7cd2a2d466b253948d799330")] = "Einzigartiger Name",
                [("business-objects-create", "c7874aaa0fab4eeff4f2db2ec37d5f5a2dea859a")] = "Anzeigename",
                [("business-objects-edit", "2dacf65959849884a011f36f76a04eebea94c5ea")] = "Speichern",
                [("business-objects-edit", "7bed1c54a620afe2eec2d8d4bea83d0f3b22da63")] = "Geschäftsobjekt bearbeiten",
                [("business-objects-edit", "8222c902ae0723e8bb352bb17ed76809e4688165")] = "Inspector",
                [("business-objects-edit", "b5d8af1730d32bfc7cd2a2d466b253948d799330")] = "Einzigartiger Name",
                [("business-objects-edit", "c7874aaa0fab4eeff4f2db2ec37d5f5a2dea859a")] = "Anzeigename",
                [("business-objects-view", "0d7c240cba1cf038ff57e08f553a5b87e790fa6f")] = "Inspektionen",
                [("business-objects-view", "5301648dcf6b53cefc9ed52999aaa92d4603cae0")] = "Ändern",
                [("business-objects-view", "5714d2e05481e6295c012970c9ec8ca7587e0b3b")] = "Audithistorie",
                [("business-objects-view", "6e157c5da4410b7e9de85f5c93026b9176e69064")] = "Erstellen",
                [("business-objects-view", "8222c902ae0723e8bb352bb17ed76809e4688165")] = "Inspektor",
                [("business-objects-view", "8bda3b551a97c3abb13df026df3e68f337f82762")] = "Geschäftsobjekte",
                [("business-objects-view", "b5d8af1730d32bfc7cd2a2d466b253948d799330")] = "Einzigartiger Name",
                [("business-objects-view", "c7874aaa0fab4eeff4f2db2ec37d5f5a2dea859a")] = "Anzeigename",
                [("business-objects-view", "e57016edceec7e7b4449fe027bb35d1ba319eb9f")] = "Nicht zugewiesen",
                [("business-objects-view", "f6fdbe48dc54dd86f63097a03bd24094dedd713a")] = "Löschen",
                [("change-secret", "2dacf65959849884a011f36f76a04eebea94c5ea")] = "Speichern",
                [("change-secret", "8c684290ce0b8a8e6cee79a145e17554064a4760")] = "Passwort ändern",
                [("change-secret", "d850ee188c7c55b64bc3624534de5c5051a57dc6")] = "Neues Passwort",
                [("change-secret", "e3053fbaa7622ccae5f59565e7fb553f9c2d79c8")] = "Altes Passwort",
                [("index", "ca4f9dcf204e2037bfe5884867bead98bd9cbaf8")] = "Willkommen",
                [("inspections-create", "2dacf65959849884a011f36f76a04eebea94c5ea")] = "Speichern",
                [("inspections-create", "a0b861f6121344b631992c8252fa8748835e4df6")] = "Aktiviert",
                [("inspections-create", "b5d8af1730d32bfc7cd2a2d466b253948d799330")] = "Einzigartiger Name",
                [("inspections-create", "c3328c39b0e29f78e9ff45db674248b1d245887d")] = "Text",
                [("inspections-create", "c7874aaa0fab4eeff4f2db2ec37d5f5a2dea859a")] = "Anzeigename",
                [("inspections-create", "e3db3b6927275b4ea32469fcad89a136c7e4129c")] = "Inspektion anlegen",
                [("inspections-edit", "2dacf65959849884a011f36f76a04eebea94c5ea")] = "Speichern",
                [("inspections-edit", "a0b861f6121344b631992c8252fa8748835e4df6")] = "Aktiviert",
                [("inspections-edit", "b5d8af1730d32bfc7cd2a2d466b253948d799330")] = "Einzigartiger Name",
                [("inspections-edit", "bba5c7128862ddd921c16a3194cd5c55c9f3ead9")] = "Inspektion bearbeiten",
                [("inspections-edit", "c3328c39b0e29f78e9ff45db674248b1d245887d")] = "Text",
                [("inspections-edit", "c7874aaa0fab4eeff4f2db2ec37d5f5a2dea859a")] = "Anzeigename",
                [("inspections-view", "0d7c240cba1cf038ff57e08f553a5b87e790fa6f")] = "Inspektionen",
                [("inspections-view", "5301648dcf6b53cefc9ed52999aaa92d4603cae0")] = "Ändern",
                [("inspections-view", "6e157c5da4410b7e9de85f5c93026b9176e69064")] = "Erstellen",
                [("inspections-view", "b5d8af1730d32bfc7cd2a2d466b253948d799330")] = "Einzigartiger Name",
                [("inspections-view", "c7874aaa0fab4eeff4f2db2ec37d5f5a2dea859a")] = "Anzeigename",
                [("inspections-view", "d65ded94286c61f4f34527624be77ef1afce3b5a")] = "Deaktivieren",
                [("inspections-view", "f6fdbe48dc54dd86f63097a03bd24094dedd713a")] = "Löschen",
                [("inspectors-create", "2dacf65959849884a011f36f76a04eebea94c5ea")] = "Speichern",
                [("inspectors-create", "7e5a975b6add84fd53e3710a9ceac15eb06663b7")] = "Identität",
                [("inspectors-edit", "2890dd208245181013f450b92c4e2c9cf738d2bd")] = "Inspektor bearbeiten",
                [("inspectors-edit", "2dacf65959849884a011f36f76a04eebea94c5ea")] = "Speichern",
                [("inspectors-edit", "7e5a975b6add84fd53e3710a9ceac15eb06663b7")] = "Identität",
                [("inspectors-edit", "a0b861f6121344b631992c8252fa8748835e4df6")] = "Aktiviert",
                [("inspectors-edit", "b5d8af1730d32bfc7cd2a2d466b253948d799330")] = "Einzigartiger Name",
                [("inspectors-view", "5301648dcf6b53cefc9ed52999aaa92d4603cae0")] = "Ändern",
                [("inspectors-view", "6e157c5da4410b7e9de85f5c93026b9176e69064")] = "Erstellen",
                [("inspectors-view", "7e5a975b6add84fd53e3710a9ceac15eb06663b7")] = "Identität",
                [("inspectors-view", "ad1128fcabfd66156dd2f450d19a55c5c893b73b")] = "Inspektoren",
                [("inspectors-view", "b5d8af1730d32bfc7cd2a2d466b253948d799330")] = "Einzigartiger Name",
                [("inspectors-view", "d65ded94286c61f4f34527624be77ef1afce3b5a")] = "Deaktivieren",
                [("inspectors-view", "f6fdbe48dc54dd86f63097a03bd24094dedd713a")] = "Löschen",
                [("main-layout", "49289db43e663a3df5e2c70714722ecc54895565")] = "Passwort ändern",
                [("main-layout", "61fd08ff5c37211d36177662f64c208ba938b315")] = "Abmelden",
                [("main-layout", "cc93f34f921a9b60c004893a42230ba323335950")] = "Zur Homepage",
                [("main-layout", "cef2fe093b68fa970c7d3d9b72c1699814a06f7d")] = "Wiederholen",
                [("missing-translations", "054256ee41aa93ade4e54579990f6f9f8b65a4e9")] = "Fehlende Übersetzungen",
                [("nav-menu", "018514a3d58aa08353dd5e387ee29de45981c409")] = "Timeline",
                [("nav-menu", "054256ee41aa93ade4e54579990f6f9f8b65a4e9")] = "Fehlende Übersetzungen",
                [("nav-menu", "090ec5f560fc50377fcd95e5cda128e91b276e98")] = "Aufgaben",
                [("nav-menu", "0b81497c8589af2714a1ddbeb62b3a4be679c318")] = "Registrieren",
                [("nav-menu", "0d7c240cba1cf038ff57e08f553a5b87e790fa6f")] = "Inspektionen",
                [("nav-menu", "0eb5ed506e4923c28d7f4a8aa69efe99b3ad75d1")] = "Informationen",
                [("nav-menu", "24ce83115fb7f77f6418945028c9e53eba1053b9")] = "Geschäftsobjekte",
                [("nav-menu", "2a27af1448f2f08e35981fdfbf7a717f90f24a3e")] = "Audithistorie",
                [("nav-menu", "49289db43e663a3df5e2c70714722ecc54895565")] = "Passwort ändern",
                [("nav-menu", "63cecca67b70751fc04062673106be15df88ce5f")] = "Verwaltung",
                [("nav-menu", "753a22b2eb617204efee4644795034b8ace1ee14")] = "Benachrichtigungen",
                [("nav-menu", "85dfa32c97d8618d1bea083609e2c8a29845abe5")] = "Konto",
                [("nav-menu", "ad1128fcabfd66156dd2f450d19a55c5c893b73b")] = "Inspektoren",
                [("nav-menu", "cb5cc8e2ad78882026ea4054f5bcf86b043d26db")] = "Auditing",
                [("nav-menu", "dc1649a16c1496db3e4073be6a73faf5121aeae7")] = "Abmelden",
                [("notifications-view", "c18f8f255ab9c208f23d0340eb9dff5a84efe311")] = "Benachrichtigungen",
                [("notifications-view", "f6fdbe48dc54dd86f63097a03bd24094dedd713a")] = "Löschen",
                [("sign-in", "2dacf65959849884a011f36f76a04eebea94c5ea")] = "Anmelden",
                [("sign-in", "7e5a975b6add84fd53e3710a9ceac15eb06663b7")] = "Identität",
                [("sign-in", "8be3c943b1609fffbfc51aad666d0a04adf83c9d")] = "Passwort",
                [("sign-in", "ada2e9e96fa9ce85430225b412068b5b62b79800")] = "Anmeldung",
                [("sign-in-inspector", "c0d15a298e19ae328ef8f097acf633e35bd89ea3")] = "Inspektoranmeldung",
            };
    }
}
