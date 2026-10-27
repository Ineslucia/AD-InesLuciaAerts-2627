# Capstoneproject — Concert & Event Ticketing

## 1. Scenario

Concert & Event Ticketing is een toepassing waarmee concerten en evenementen beheerd kunnen worden. Gebruikers kunnen evenementen bekijken en tickets aanvragen. Elk evenement heeft een maximale capaciteit. Het systeem houdt bij hoeveel tickets er aangemaakt zijn en voorkomt dat er meer tickets worden uitgegeven dan er plaatsen beschikbaar zijn.

De toepassing bestaat uit een ASP.NET Web API als back-end en een React-applicatie als front-end.

## 2. Datamodel

Het project bevat twee domeinentiteiten: `Event` en `Ticket`.

### 2.1 Event

Een `Event` stelt een concert of evenement voor.

| Veld | Type | Uitleg |
|---|---|---|
| Id | int | Unieke identificatie van het evenement |
| Name | string | Naam van het evenement |
| Description | string | Beschrijving van het evenement |
| Location | string | Locatie waar het evenement plaatsvindt |
| EventDate | DateTime | Datum en tijd van het evenement |
| Capacity | int | Maximumaantal tickets voor het evenement |

### 2.2 Ticket

Een `Ticket` stelt één ticket voor dat bij een evenement hoort.

| Veld | Type | Uitleg |
|---|---|---|
| Id | int | Unieke identificatie van het ticket |
| EventId | int | Foreign key naar het bijbehorende evenement |
| HolderName | string | Naam van de tickethouder |
| HolderEmail | string | E-mailadres van de tickethouder |
| TicketType | string | Type ticket, bijvoorbeeld Regular of VIP |
| Price | decimal | Prijs van het ticket in euro |
| CreatedAt | DateTime | Datum en tijd waarop het ticket werd aangemaakt |

`Id` en `CreatedAt` worden door de server beheerd. De client mag deze waarden niet zelf bepalen.

### 2.3 Relatie tussen de entiteiten

- Eén `Event` kan meerdere `Tickets` hebben.
- Eén `Ticket` hoort bij precies één `Event`.
- `Ticket.EventId` is de foreign key die naar `Event.Id` verwijst.
- Een evenement kan worden opgevraagd met de bijbehorende tickets.

De relatie is dus **één-op-veel (1-N)**.

## 3. Businessregel

### Maximale capaciteit van een evenement

Een evenement mag nooit meer tickets bevatten dan de ingestelde capaciteit.

Wanneer een nieuw ticket wordt aangemaakt, controleert de service-laag hoeveel tickets er al voor het evenement bestaan.

- Als er nog plaatsen beschikbaar zijn, wordt het ticket aangemaakt.
- Als de maximale capaciteit bereikt is, wordt het ticket niet aangemaakt.
- Als het evenement niet bestaat, wordt het ticket niet aangemaakt.

Bij overtreding van de capaciteitsregel geeft de API HTTP-statuscode **409 Conflict** terug.

**Voorbeeld:**

Een evenement heeft een capaciteit van 100 tickets. Er zijn al 100 tickets aangemaakt. Wanneer een gebruiker een extra ticket probeert aan te maken, weigert het systeem de aanvraag.

De capaciteitscontrole wordt uitgevoerd in de service-laag en niet uitsluitend in de DTO-validatie.

## 4. Validatieregels

### 4.1 Event

| Veld | Validatieregel |
|---|---|
| Name | Verplicht en mag niet leeg zijn |
| Description | Verplicht en mag niet leeg zijn |
| Location | Verplicht en mag niet leeg zijn |
| EventDate | Verplicht en moet in de toekomst liggen bij het aanmaken |
| Capacity | Verplicht en moet groter zijn dan 0 |

### 4.2 Ticket

| Veld | Validatieregel |
|---|---|
| EventId | Verplicht en moet verwijzen naar een bestaand evenement |
| HolderName | Verplicht en mag niet leeg zijn |
| HolderEmail | Verplicht en moet een geldig e-mailadres bevatten |
| TicketType | Verplicht en mag niet leeg zijn |
| Price | Verplicht en moet groter dan of gelijk aan 0 zijn |

`Id` en `CreatedAt` worden door de server ingevuld en worden niet door de client meegestuurd.

De validatie van verplichte velden en toegelaten waarden gebeurt via DTO's. De capaciteitsregel wordt afzonderlijk in de service-laag gecontroleerd.

## 5. Seed-data

Bij het opstarten van de applicatie wordt de database automatisch gevuld met voorbeeldgegevens.

De seed-data bevat minimaal:

- Vier evenementen met verschillende namen, datums, locaties en capaciteiten.
- Drie tickets, verdeeld over minstens twee evenementen.
- Eén evenement waarvan de capaciteit al bereikt is, zodat de businessregel getest kan worden.

Voorbeeld:

| Evenement | Capaciteit | Aantal bestaande tickets |
|---|---:|---:|
| K-Pop Night | 2 | 2 |
| Summer Music Festival | 100 | 1 |
| Rock Night | 200 | 0 |
| Acoustic Evening | 50 | 0 |

De seed-data wordt aangemaakt met een `DbInitializer`. De evenementen worden eerst opgeslagen, zodat de tickets aan bestaande evenementen gekoppeld kunnen worden.

## 6. Authenticatie en toegangsrechten

De toepassing gebruikt JWT-authenticatie.

- **Zonder inloggen:** gebruikers kunnen evenementen en bijbehorende tickets bekijken.
- **Na inloggen:** gebruikers kunnen nieuwe tickets aanmaken.
- Het aanmaken van tickets vereist een geldig JWT-token.
- Wachtwoorden worden nooit in platte tekst opgeslagen, maar als hash.

De API controleert de authenticatie bij beveiligde endpoints.

## 7. Schermen van de front-end

### 7.1 Eventoverzicht

Een overzicht van alle evenementen met hun naam, datum, locatie en capaciteit.

### 7.2 Ticketoverzicht

Een overzicht van alle tickets met de tickethouder, het tickettype, de prijs en het bijbehorende evenement.

### 7.3 Nieuw ticket

Een formulier waarmee een ingelogde gebruiker een ticket kan aanmaken.

Het formulier bevat velden voor de naam, het e-mailadres, het tickettype, de prijs en een selectie van een bestaand evenement.

De gebruiker krijgt een foutmelding wanneer verplichte velden ontbreken of wanneer de API het ticket weigert omdat het evenement volzet is.

### 7.4 Inloggen

Een formulier met gebruikersnaam en wachtwoord. Na een succesvolle login wordt het JWT-token bewaard en gebruikt bij beveiligde API-verzoeken.

## 8. Front-endvalidatie

De React-applicatie controleert de invoer voordat een aanvraag naar de API wordt verstuurd.

- Verplichte velden moeten ingevuld zijn.
- Het e-mailadres moet een geldig formaat hebben.
- De prijs moet groter dan of gelijk aan 0 zijn.
- Er moet een bestaand evenement geselecteerd zijn.
- Bij een fout van de API, zoals `409 Conflict`, krijgt de gebruiker een duidelijke foutmelding.

De back-end blijft verantwoordelijk voor de definitieve validatie en de businessregels.

## 9. Technische uitwerking

- **Back-end:** ASP.NET Web API
- **Front-end:** React
- **Database:** SQLite
- **ORM:** Entity Framework Core
- **Authenticatie:** JWT
- **Architectuur:** Controller / Service / Repository
- **Validatie:** DTO's en client-side validatie
- **Database-initialisatie:** Entity Framework migrations en seeding

## 10. Mogelijke uitbreidingen

Als de basisfunctionaliteit volledig werkt, kunnen de volgende uitbreidingen worden toegevoegd:

- Zoeken en filteren op evenementen.
- Evenementdetails met alle bijbehorende tickets.
- Ticketverkoopstatistieken.
- Verschillende gebruikersrollen, zoals organisator en bezoeker.
- Een verzorgde, responsieve vormgeving.
