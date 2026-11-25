# Five Nights at HTL Grieskirchen – Game Design Document (GDD)

Dieses Dokument dient als vollständige Projektgrundlage und Übergabe-Dokumentation für die Entwicklung des Spiels **„Five Nights at HTL Grieskirchen“** in Unity. Es fasst alle Anforderungen, Spielmechaniken, Gegner, UI-Konzepte, Balancing-Regeln und den 12-Phasen-Entwicklungsplan zusammen.

---

## 1. Projekt-Rahmenbedingungen & Arbeitsweise (WICHTIG!)

* **Kontext:** Schulprojekt für das Fach *Game Development* (HTL Grieskirchen).
* **Engine & Genre:** Unity (2D Point-and-Click Survival-Horror, stark orientiert an *Five Nights at Freddy's 1* mit Elementen aus *FNAF 2*, *FNAF 3* und *Ultimate Custom Night*).
* **Erfahrungsstand des Entwicklers:** **Absoluter Unity- und Programmier-Anfänger.**
  * **Anforderung an Anleitungen:** Jede Anleitung muss extrem präzise und kleinschrittig formuliert sein. Es muss **immer exakt angegeben werden**, wo per Rechtsklick etwas erstellt wird, wie Objekte/Ordner heißen, wo Dateien gespeichert werden, welche Komponenten im Inspector zugewiesen werden und wohin welcher Code gehört.
* **STRIKTE CODE-REGEL:** Sämtlicher C#-Code muss **IMMER komplett ohne Kommentare** (keine `//` und keine `/* ... */`) geschrieben werden.
* **Rechtlicher Hinweis:** Alle im Spiel vorkommenden realen Personen (die Lehrer der HTL Grieskirchen) haben ausdrücklich ihr Einverständnis gegeben, im Spiel verwendet zu werden.

---

## 2. Spielstruktur, Grafikstil & Fortschritt

### 2.1 Visueller Stil & Assets
* **Echte Fotos:** Das Office, die Kameraräume und die Lehrer bestehen aus echten Handyfotos im PNG-Format.
* **Lehrer auf den Kameras:** Alle auf den Cams sichtbaren Lehrer werden als eigene, **leicht transparente PNG-Overlays** über das jeweilige Raumbild gelegt.
* **Platzhalter-Prinzip:** Für noch fehlende Raumfotos sowie für UI-Buttons und Icons werden zu Beginn saubere Platzhalter verwendet. Der Entwickler erstellt eigene Button-/Icon-Sprites separat; das Unity-UI muss daher so aufgebaut werden, dass Platzhalter später ohne Code-Änderungen per Drag-and-Drop ausgetauscht werden können.
* **Zielauflösung:** Festes 16:9-Format ($1920 \times 1080$) mit sauberem Canvas-Scaling.

### 2.2 Zeit- & Nacht-System
* **Nachtlänge:** Exakt **4:30 Minuten** ($270\text{ Sekunden}$).
  * Bei einer klassischen Anzeige von 0:00 Uhr (12 AM) bis 6:00 Uhr (6 AM) entspricht eine In-Game-Stunde genau $45\text{ Sekunden}$.
* **Kampagnen-Fortschritt:**
  * **Nächte 1 bis 5 (Hauptnächte):** Kumulativer Aufbau. Jede Nacht führt neue Lehrer ein; alle Lehrer aus vorherigen Nächten bleiben aktiv und erhalten leicht höhere AI-Level.
  * **Nacht 6:** Zusatznacht mit erhöhtem Schwierigkeitsgrad (gleiches Roster wie Nacht 5, höhere AI).
  * **Custom Night (CN):** Wird freigeschaltet, sobald alle 6 Nächte bestanden wurden. Alle **8 regulären Lehrer** sind frei einstellbar; **Alex** bleibt als passiver Gegner dauerhaft aktiv und kann nicht an- oder ausgeschaltet werden.

---

## 3. Office-Mechaniken, Balancing & Jumpscare-Logik

### 3.1 Das Office
* Der Spieler bleibt dauerhaft im Office sitzen (keine freie Bewegung zwischen Räumen).
* **Tür & Licht (1 Eingang):**
  * **Tür:** Wird durch Klick direkt in den Türrahmen geöffnet bzw. geschlossen.
  * **Licht:** Wird durch Klick auf einen Knopf am Rand der Tür ein- bzw. ausgeschaltet.
  * **Visuelle Umsetzung:** 4 separate Office-PNGs für alle Kombinationen:
    1. Tür auf, unbeleuchtet
    2. Tür zu, unbeleuchtet
    3. Tür auf, beleuchtet
    4. Tür zu, beleuchtet
* **Stundenplan:** Hängt sichtbar im Office an der Wand. Ein Klick darauf öffnet eine Großansicht, in der abgelesen werden kann, wann die aktuelle Mathestunde (für Wiesinger) endet.

### 3.2 Anti-Camping-Balancing (Tür & Licht)
* **Temperatur-Anstieg:** Bleibt die Tür längere Zeit geschlossen, steigt die Gebäudetemperatur **schneller als regulär ohne Ventilation**. Der Spieler hat genug Zeit, um Gegner sauber abzublocken, kann aber selbst bei eingeschalteter Ventilation nicht endlos mit geschlossener Tür „stallen“.
* **Internetverbrauch:** Eine geschlossene Tür (sowie aktive Systeme) erhöht zusätzlich den Internetverbrauch und beschleunigt damit das Abfallen der Internet-Leiste (Hable Krispin).

### 3.3 Jumpscare-Verhalten (Wichtiger Unterschied zu FNAF 1 / UCN!)
* **Direkte Timer-Logik:** Gegner warten **nicht** darauf, dass der Spieler erst den PC-Monitor wieder herunterklappt.
* **Beispiel Raab:** Steht Raab an der Tür und bleibt diese zu lange offen, löst er **sofort** den Jumpscare aus – unabhängig davon, ob der Spieler gerade im Office, am PC oder am Handy ist. Offene Screens werden beim Auslösen eines Jumpscares unmittelbar abgebrochen/geschlossen.

---

## 4. PC-Screen, Kameras, Tools & Handy-System

Es gibt zwei voneinander getrennte Zusatz-Screens im Office (PC und Handy), die nicht gleichzeitig geöffnet sein können.

### 4.1 Der PC-Screen (2 Tabs)
Der PC öffnet sich als eigener Overlay-Screen (FNAF-Monitor-Prinzip) mit zwei umschaltbaren Tabs:

#### Tab 1: Cams (11 Kameras)
Ein Klick auf eine Kamera auf der Schul-Map wechselt den Raum-Hintergrund sowie die aktuell dort aktiven (leicht transparenten) Lehrer-Overlays. Aktuelle Raumliste:
1. Ostflur
2. Westflur
3. 4C
4. Aula
5. Treppenhaus
6. Lehrerzimmer
7. Lehrerkaffeezimmer
8. Oberer Westflur
9. NSCS Raum *(wichtig für Hable Krispin)*
10. NAWI Raum
11. Serverraum *(oder kleines Nebenzimmer – wichtig für Doppler)*

*(Fast alle Raumfotos liegen bereits vor; fehlende Räume erhalten zunächst Platzhalter.)*

#### Tab 2: Tools (Exklusiv-System)
Es gibt **4 Tools**. Es kann **immer nur maximal 1 Tool gleichzeitig** aktiv sein (oder gar keins). Das Nutzen von Tools verbraucht Internet.
1. **Ventilation:** Senkt die Gebäudetemperatur.
2. **Heizung:** Erhöht die Gebäudetemperatur.
3. **VPN:** Senkt den allgemeinen Internetverbrauch.
4. **Motion Detector:** Zeigt kurz ein visuelles Warnsignal direkt beim PC-Button an, sobald sich ein Lehrer bewegt – **dies gilt ausdrücklich auch für Kimmerstorfer**, obwohl er auf den Kameras unsichtbar ist.

### 4.2 Der Handy-Screen (Reparatur-System nach FNAF 3)
* Separater Screen zum Reparieren von ausgefallenen Tools/Systemen.
* **Reparatur-Buttons:**
  * Jedes der 4 Tools besitzt einen eigenen **Repair-Button**.
  * Zusätzlich gibt es einen **„Alle reparieren“-Button**, dessen Dauer sich wie folgt berechnet:
    $$\text{Gesamtdauer} = \text{Einzel-Reparaturzeit} \times \text{Anzahl der defekten Tools}$$
* **Abbruch-Regel:** Während eine Reparatur läuft, darf das Handy **nicht** weggelegt werden. Wird das Handy vorzeitig geschlossen, bricht der laufende Reparaturvorgang ab.
* **Schwierigkeits-Skalierung:** Je weiter die Nacht voranschreitet (Nacht 1 bis 6), desto länger dauern die Reparaturen.

---

## 5. UI- & Visuelles Design

Das Interface soll atmosphärisch, modern und unter Zeitdruck auf einen Blick lesbar sein (ohne überladen zu wirken).

* **Temperaturanzeige (unten rechts):**
  * Wird in **Grad Fahrenheit ($^\circ\text{F}$)** angezeigt.
  * **Dynamische Farben & Icons:**
    * *Neutral / Normal:* Grau.
    * *Aktiv gekühlt (Ventilation) / kühl:* **Hellblau** zusammen mit einem **Freezing-Icon** (Schneeflocke/Frost).
    * *Aktiv geheizt (Heizung) / warm:* **Rot**.
    * *Kritisch heiß (Gefahr durch Wendt):* **Dunkelrot** (optional mit Warn-Effekt/Pulsieren).
    * *Analog für kritische Kälte:* Intensives Eisblau/Warn-Indikator (da Wendt auch bei zu kalter Temperatur angreift).
* **Weitere HUD-Elemente:**
  * Uhrzeit (12 AM – 6 AM) und aktuelle Nacht oben rechts.
  * Internet-Statusanzeige (für Hable Krispin / Ressourcen-Management).
  * Motion-Detector-Warnsymbol direkt am PC-Button.
  * Austauschbare UI-Sprites für alle Buttons und Status-Icons.

---

## 6. Gegner-Roster & Nacht-Einteilung

Insgesamt gibt es **9 Gegner**: **8 reguläre Lehrer** (in der Custom Night einstellbar) plus **Alex als 9. passiven Gegner**.

### 6.1 Übersicht aller Gegner

| Gegner | Typ | Ab Nacht | Mechanik & Abwehr |
| :--- | :--- | :---: | :--- |
| **Alex** | Passiv (immer aktiv, in CN fix) | Nacht 1 (immer) | Agiert passiv wie *The Puppet* in FNAF 2 für die Technik: Bleiben defekte Tools/Systeme (z. B. Ventilation) zu lange unrepariert, löst Alex einen Jumpscare aus. |
| **Raab** | Tür-Gegner (Cam-sichtbar) | Nacht 1 | Bewegt sich über die Kameras zum Office. Muss per **geschlossener Tür** abgeblockt werden. Bleibt die Tür bei seiner Anwesenheit zu lange offen, folgt ein direkter Jumpscare. |
| **Kasbauer** | Licht-Gegner | Nacht 1 | Muss per **Türlicht** abgeblockt/verscheucht werden. Die geschlossene Tür hilft gegen ihn **nicht**. |
| **Hable Krispin** | Ressourcen / Music-Box | Nacht 2 | Überwacht das Internet. Sinkt das Internet zu weit ab, muss es über die Cam im **NSCS Raum** manuell wieder aufgeladen werden (Music-Box-Prinzip). Das Nutzen von Cams, Tools und Tür zieht Internet und beschleunigt den Abfall (VPN verlangsamt ihn). |
| **Wiesinger** | Cam-Sammelaufgabe & Stundenplan | Nacht 2 | Auf verschiedenen Cams spawnen **nacheinander** Mathe-Mitschriften, die per Klick eingesammelt werden müssen, bevor die Mathestunde endet. Das Stundenende wird zu Beginn der Nacht zufällig gewürfelt (z. B. 3 Uhr / ca. 2:30 Min.) und ist über den **Stundenplan im Office** einsehbar. Höhere AI = mehr Mitschriften, früheres Stundenende und mehrere Phasen pro Nacht (z. B. Phase 1 bis 1 Uhr, danach neue Mitschriften für Phase 2 bis 4 Uhr). |
| **Planberger** | Audio-Reaktion | Nacht 3 | Spielt eine von zwei Voicelines ab: **„Mochts die Laptops zu“** (Spieler muss *aus* den Cams raus) oder **„Schauma uns die Hausübung an“** (Spieler muss *in* die Cams rein). Sehr kurzes Reaktionsfenster; höhere AI = noch weniger Zeit. |
| **Wendt** | Temperatur-Gegner | Nacht 4 | Reagiert auf die Gebäudetemperatur ($^\circ\text{F}$). Wird es **zu heiß oder zu kalt**, greift er an. Spieler muss mit Ventilation und Heizung die Balance halten. |
| **Kimmerstorfer** | Audio- & Tür-Gegner (unsichtbar) | Nacht 4 | Auf den Kameras **nicht sichtbar**. Muss über **Audiolines** geortet werden (bestimmte Sätze deuten auf seinen Raum hin, die Lautstärke zeigt seine Nähe an). Löst trotzdem den **Motion Detector** aus. Muss per **geschlossener Tür** abgeblockt werden. |
| **Doppler** | Minigame-Gegner (UCN Toy Freddy) | Nacht 5 & 6 | Sitzt dauerhaft in einer festen Cam (**Serverraum / Nebenraum**) und spielt Videospiele. Der Spieler muss direkt in seiner Cam ein **Minigame** spielen, um ihn vor dem Verlieren zu schützen. Verliert Doppler, jumpscared er den Spieler. |

### 6.2 Nacht-Progression (kumulativ)
Alle Lehrer der vorherigen Nächte bleiben jeweils aktiv und erhalten steigende AI-Werte:
* **Immer aktiv (Passiv):** Alex
* **Nacht 1:** Raab, Kasbauer
* **Nacht 2:** + Hable Krispin, Wiesinger
* **Nacht 3:** + Planberger
* **Nacht 4:** + Wendt, Kimmerstorfer
* **Nacht 5:** + Doppler
* **Nacht 6:** Alle Gegner aktiv mit höherer AI (Zusatznacht)
* **Custom Night:** Nach Abschluss von Nacht 6 freigeschaltet – 8 AI-Regler (Raab, Kasbauer, Hable Krispin, Wiesinger, Planberger, Wendt, Kimmerstorfer, Doppler) + Alex fix aktiv.

---

## 7. Entwicklungs-Roadmap (12 Phasen)

Die Entwicklung erfolgt strikt modular in 12 überschaubaren Phasen. Nach jeder Phase ist das Projekt im Unity-Editor direkt testbar. Die Phasen 1 und 2 enthalten noch keinerlei Programmierung.

1. **Phase 1 – Unity-Setup & Ordnerstruktur (kein Code):**
   2D-Projekt erstellen, Unity-Oberfläche kennenlernen, feste $1920 \times 1080$-Auflösung und Canvas-Scaling einrichten, saubere Ordnerstruktur (`Sprites/Office`, `Sprites/Cams`, `Sprites/Lehrer`, `Sprites/UI`, `Audio`, `Scripts`, `Prefabs`, `Scenes`) anlegen sowie vorhandene Handyfotos und erste Platzhalter importieren.
2. **Phase 2 – Visuelles UI-Layout (kein Code):**
   Office-Szene (Tür-Klickzone, Lichtschalter, Stundenplan), PC-Overlay (Tabs *Cams* & *Tools*, 11-Raum-Map, transparenter Lehrer-Layer), Handy-Overlay (Reparatur-Buttons & Fortschrittsbalken), HUD (Uhr, Internet, Motion-Detector-Icon, dynamische $^\circ\text{F}$-Temperaturanzeige unten rechts) sowie ausgeblendete Jumpscare-/6-AM-Panels visuell aufbauen.
3. **Phase 3 – Office-Steuerung, Screen-Manager & Nacht-Timer:**
   Erste C#-Scripts (ohne Kommentare): Umschalten der 4 Office-PNGs über Tür & Licht, gegenseitiges Sperren/Öffnen von PC, Handy und Stundenplan, der $4:30$-Minuten-Timer ($45\text{ Sekunden}$ pro Stunde) bis 6 AM sowie das zentrale Jumpscare-System (bricht offene Screens sofort ab).
4. **Phase 4 – Kamera-System (11 Cams & Lehrer-Overlays):**
   Kamera-Umschaltung per Map-Buttons implementieren und das dynamische Ein-/Ausblenden der leicht transparenten Lehrer-PNGs pro aktivem Raum vorbereiten.
5. **Phase 5 – Temperatur- ($^\circ\text{F}$), Internet- & Tool-System:**
   Temperatur-Logik mit automatischem Farb- & Icon-Wechsel (Grau, Hellblau + Freezing-Icon, Rot, Dunkelrot), schnellerer Hitzestau und erhöhter Internetverbrauch bei geschlossener Tür/Licht sowie die 4 exklusiven Tools (Ventilation, Heizung, VPN, Motion Detector – maximal 1 aktiv).
6. **Phase 6 – Handy-Reparatur & Alex (Passiv-Gegner):**
   Zufällige Tool-Ausfälle, Reparatur-Timer pro Tool + „Alle reparieren“ ($\text{Dauer} \times \text{Anzahl defekter Tools}$), sofortiger Abbruch beim Schließen des Handys, Skalierung der Reparaturzeit pro Nacht und Integration von Alex als permanentem Hintergrund-Gegner.
7. **Phase 7 – Wegpunkte, Motion Detector, Raab & Kasbauer (Nacht 1):**
   Wiederverwendbares Routen-/Wegpunkte-System über die 11 Cams, Auslösen des Motion-Detector-Warnsignals am PC-Button bei Lehrer-Bewegung sowie komplette Umsetzung von Raab (direkter Tür-Gedulds-Timer) und Kasbauer (Licht-Abwehr).
8. **Phase 8 – Hable Krispin & Wendt (Ressourcen-Gegner):**
   Auflade-Button für das Internet im **NSCS Raum** (Music-Box-Prinzip) für Hable Krispin sowie Wendts Gefahren-Timer bei zu hoher oder zu niedriger $^\circ\text{F}$-Temperatur.
9. **Phase 9 – Wiesinger & der Office-Stundenplan (Nacht 2):**
   Zufällige Berechnung der Mathestunden-Endzeiten inkl. Anzeige am Office-Stundenplan, sequenzielles Spawnen anklickbarer Mathe-Mitschriften in den Cams sowie Mehrphasen-System für höhere AI-Stufen.
10. **Phase 10 – Planberger & Kimmerstorfer (Audio-Gegner, Nacht 3 & 4):**
    Planbergers Voicelines (*„Mochts die Laptops zu“* / *„Schauma uns die Hausübung an“*) mit AI-abhängigem Reaktionsfenster sowie Kimmerstorfers unsichtbare Route mit Raum-Voicelines, Distanz-Lautstärke, Motion-Detector-Trigger und direktem Tür-Timer.
11. **Phase 11 – Doppler & das Serverraum-Minigame (Nacht 5 & 6):**
    Kompaktes UI-Minigame direkt in der Serverraum-Cam (mit Platzhaltern), damit der Spieler Doppler vor dem Verlieren rettet – inklusive eigenem Verlust-Timer und Jumpscare.
12. **Phase 12 – Hauptmenü, Custom Night, Asset-Swap & Export:**
    Titelbildschirm, Speichersystem für den Nacht-Fortschritt (Nächte 1–6), zentrale AI-Konfigurationstabelle, Custom Night (8 Lehrer-Slider + Alex fix aktiv), Austausch aller Platzhalter gegen finale Grafiken/Sounds, optionaler Feinschliff (Cam-Static, Screen-Shake) und Build als spielbare `.exe`.

---

## 8. Noch gemeinsam festzulegende Details (in den jeweiligen Phasen)

Folgende Feinheiten werden bewusst erst in der jeweils passenden Entwicklungsphase gemeinsam definiert, um die Komplexität klein zu halten:
* Exakte **Kamera-Routen** für Raab, Kasbauer und Kimmerstorfer über die 11 Räume.
* Genaue **Balancing-Zahlen** (Starttemperatur in $^\circ\text{F}$, kritische Hitze-/Kältegrenzen für Wendt, exakter Temperaturanstieg bei geschlossener Tür, Internet-Verbrauchsraten, Reparaturdauer pro Nacht).
* Konkretes **AI-Scaling** (0–20) für jeden Lehrer in den Nächten 1 bis 6.
* Konkreter Ablauf & Layout des **Doppler-Minigames** im Serverraum.
* Konkrete **Audio-Dateien** (Voicelines für Planberger & Kimmerstorfer, Jumpscare-Sounds, Tür-/Klick-/Kamera-Effekte).