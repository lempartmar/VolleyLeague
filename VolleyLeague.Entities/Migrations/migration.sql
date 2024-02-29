DELETE FROM [tomasz1_voladmin].[ForumTopics]
DELETE FROM [tomasz1_voladmin].[ForumCategories]
delete from  [Articles]
delete from [tomasz1_voladmin].[Comments]
delete from  [tomasz1_voladmin].[CommentLocations]
DELETE FROM [tomasz1_voladmin].[Invitations]
DELETE FROM [tomasz1_voladmin].[Logs]
DELETE FROM [tomasz1_voladmin].[Rounds]
DELETE FROM [tomasz1_voladmin].[Seasons]
DELETE FROM [tomasz1_voladmin].[Matches]

DELETE FROM [tomasz1_voladmin].[TeamPlayers]
DELETE FROM [tomasz1_voladmin].[Teams]
delete from positions
DELETE FROM [tomasz1_voladmin].[Leagues]
DELETE FROM [tomasz1_voladmin].[Users]
DELETE FROM [tomasz1_voladmin].[SportsVenues]
DELETE FROM [tomasz1_voladmin].[PersonalLogs]
DELETE FROM [tomasz1_voladmin].[TypedResults]





-- Enable IDENTITY_INSERT for the new table
SET IDENTITY_INSERT positions ON

-- Insert data from old table to new table
INSERT INTO positions (
    [Id], 
    [Name]
)
SELECT 
    idpozycji, 
    CAST([nazwa] AS NVARCHAR(MAX))
FROM [tomasz1_ligasiatkowki].[dbo].Pozycja

-- Disable IDENTITY_INSERT after the operation
SET IDENTITY_INSERT positions OFF



-- Truncate the new table

-- Enable IDENTITY_INSERT for the new table
SET IDENTITY_INSERT [tomasz1_voladmin].[CommentLocations] ON

-- Insert data from old table to new table
INSERT INTO [tomasz1_voladmin].[CommentLocations] (
    [Id], 
    [Name]
)
SELECT 
    [idMiejscaKomentarza], 
    CAST([nazwa] AS NVARCHAR(MAX))
FROM [tomasz1_ligasiatkowki].[dbo].[MiejsceKomentarza]

-- Disable IDENTITY_INSERT after the operation
SET IDENTITY_INSERT [tomasz1_voladmin].[CommentLocations] OFF











-- Włącz IDENTITY_INSERT dla nowej tabeli
SET IDENTITY_INSERT [tomasz1_voladmin].[Users] ON

-- Wstaw dane ze starej tabeli do nowej tabeli
INSERT INTO [tomasz1_voladmin].[Users] (
    [Id],
    [FirstName],
    [LastName],
    [AccountId],
    [BirthYear],
    [City],
    [PersonalInfo],
    [Photo],
    [Gender],
    [Height],
    [Weight],
    [JerseyNumber],
    [BlockRange],
    [AttackRange],
    [VolleyballIdol],
    [AdditionalEmail],
    [Hobby],
    [Phone],
    [PositionId],
    [PhotoWidth],
    [PhotoHeight]
)
SELECT 
    [idUzytkownika],
    CAST([imie] AS NVARCHAR(MAX)),
    CAST([nazwisko] AS NVARCHAR(MAX)),
    [idKonta],
    [rokUrodzenia],
    CAST([miasto] AS NVARCHAR(MAX)),
    CAST([infoOSobie] AS NVARCHAR(MAX)),
    [zdjecie],
    [plec],
    [wzrost],
    [waga],
    [nrNaKoszulce],
    [zasiegBlok],
    [zasiegAtak],
    CAST([siatkarskiIdol] AS NVARCHAR(MAX)),
    CAST([emailDodatkowy] AS NVARCHAR(MAX)),
    CAST([hobby] AS NVARCHAR(MAX)),
    CAST([telefon] AS NVARCHAR(MAX)),
    [idPozycji],
    [zdjecieWidth],
    [zdjecieHeight]
FROM [tomasz1_ligasiatkowki].[dbo].[Uzytkownik]

-- Wyłącz IDENTITY_INSERT po operacji
SET IDENTITY_INSERT [tomasz1_voladmin].[Users] OFF




SET IDENTITY_INSERT  [Articles] ON;

INSERT INTO [Articles] (
    [Id],
    [AuthorId],
    [Content],
    [CreationDate],
    [Title],
    [IsActive],
    [Image]
)
SELECT
    [idArtykulu],
    [idAutora],
    CAST([tresc] AS NVARCHAR(MAX)),
    [dataUtworzenia],
    CAST([tytul] AS NVARCHAR(MAX)),
    [aktywny],
    [zdjecie]
FROM [tomasz1_ligasiatkowki].[dbo].[artykul];

SET IDENTITY_INSERT  [Articles] OFF;

 






-- Enable IDENTITY_INSERT for the new table

 
SET IDENTITY_INSERT [tomasz1_voladmin].[Comments] ON

-- Insert data from old table to new table
INSERT INTO [tomasz1_voladmin].[Comments] (
    [Id], 
    [ContentId], 
    [AuthorId], 
    [Content], 
    [Active], 
    [CreationDate], 
    [CommentLocationId]
)
SELECT 
    [idKomentarza], 
    [idTresci], 
    [idAutora], 
    CAST([tresc] AS NVARCHAR(MAX)), 
    [aktywny], 
    [dataUtworzenia], 
    [idMiejscaKomentarza]
FROM [tomasz1_ligasiatkowki].[dbo].[Komentarz]

-- Disable IDENTITY_INSERT after the operation
SET IDENTITY_INSERT [tomasz1_voladmin].[Comments] OFF



-- Włącz IDENTITY_INSERT dla nowej tabeli
SET IDENTITY_INSERT [tomasz1_voladmin].[ForumCategories] ON

-- Wstaw dane ze starej tabeli do nowej tabeli
INSERT INTO [tomasz1_voladmin].[ForumCategories] (
    [Id],
    [Name]
)
SELECT 
    [idKategorii],
    CAST([nazwa] AS NVARCHAR(MAX))
FROM [tomasz1_ligasiatkowki].[dbo].[Kategoria]

-- Wyłącz IDENTITY_INSERT po operacji
SET IDENTITY_INSERT [tomasz1_voladmin].[ForumCategories] OFF



-- Włącz IDENTITY_INSERT dla nowej tabeli
SET IDENTITY_INSERT [tomasz1_voladmin].[ForumTopics] ON

-- Wstaw dane ze starej tabeli do nowej tabeli
INSERT INTO [tomasz1_voladmin].[ForumTopics] (
    [Id],
    [AuthorId],
    [Content],
    [CreationDate],
    [Title],
    [CategoryId],
    [IsActive]
)
SELECT 
    [idTematu],
    [idAutora],
    CAST([tresc] AS NVARCHAR(MAX)),
    [dataUtworzenia],
    CAST([tytul] AS NVARCHAR(MAX)),
    [idKategorii],
    [aktywny]
FROM [tomasz1_ligasiatkowki].[dbo].[Temat]

-- Wyłącz IDENTITY_INSERT po operacji
SET IDENTITY_INSERT [tomasz1_voladmin].[ForumTopics] OFF




-- Włącz IDENTITY_INSERT dla nowej tabeli
SET IDENTITY_INSERT [tomasz1_voladmin].[Leagues] ON

-- Wstaw dane ze starej tabeli do nowej tabeli
INSERT INTO [tomasz1_voladmin].[Leagues] (
[Id],
[Name]
)
SELECT
[idLigi],
[nazwa]
FROM [tomasz1_ligasiatkowki].[dbo].[Liga]

-- Wyłącz IDENTITY_INSERT po operacji
SET IDENTITY_INSERT [tomasz1_voladmin].[Leagues] OFF
 












-- Włącz IDENTITY_INSERT dla nowej tabeli
SET IDENTITY_INSERT [tomasz1_voladmin].[Teams] ON

-- Wstaw dane ze starej tabeli do nowej tabeli
INSERT INTO [tomasz1_voladmin].[Teams] (
    [Id],
    [Name],
    [CreationDate],
    [Image],
    [LeagueId],
    [Accepted],
    [CaptainId],
    [Email],
    [IsReportedToPlay],
    [ChangeCount],
    [ImageWidth],
    [ImageHeight],
    [Logo],
    [LogoWidth],
    [LogoHeight],
    [Phone],
    [TeamDescription],
    [Website],
    [PointCorrection]
)
SELECT 
    [idDruzyny],
    CAST([nazwa] AS NVARCHAR(MAX)),
    [dataUtworzenia],
    [zdjecie],
    [idLigi],
    [zaakceptowana],
    [idKapitana],
    CAST([email] AS NVARCHAR(MAX)),
    [zgloszona],
    [ileZmian],
    [zdjecieWidth],
    [zdjecieHeight],
    [logo],
    [logoWidth],
    [logoHeight],
    CAST([telefon] AS NVARCHAR(MAX)),
    CAST([opisDruzyny] AS NVARCHAR(MAX)),
    CAST([stronaWWW] AS NVARCHAR(MAX)),
    [korekcjaPunktow]
FROM [tomasz1_ligasiatkowki].[dbo].[Druzyna]

-- Wyłącz IDENTITY_INSERT po operacji
SET IDENTITY_INSERT [tomasz1_voladmin].[Teams] OFF
















-- Włącz IDENTITY_INSERT dla nowej tabeli
SET IDENTITY_INSERT [tomasz1_voladmin].[Invitations] ON

-- Wstaw dane ze starej tabeli do nowej tabeli
INSERT INTO [tomasz1_voladmin].[Invitations] (
    [Id],
    [UserId],
    [TeamId],
    [CreationDate]
)
SELECT 
    [idZaproszenia],
    [idZawodnika],
    [idDruzyny],
    [dataUtworzenia]
FROM [tomasz1_ligasiatkowki].[dbo].[Zaproszenie]

-- Wyłącz IDENTITY_INSERT po operacji
SET IDENTITY_INSERT [tomasz1_voladmin].[Invitations] OFF






-- Włącz IDENTITY_INSERT dla nowej tabeli
SET IDENTITY_INSERT [tomasz1_voladmin].[Logs] ON

-- Wstaw dane ze starej tabeli do nowej tabeli
INSERT INTO [tomasz1_voladmin].[Logs] (
[Id],
[Link],
[Description],
[Date],
[Admin]
)
SELECT
[idLogu],
[link],
[opis],
[data],
[admin]
FROM [tomasz1_ligasiatkowki].[dbo].[Log]

-- Wyłącz IDENTITY_INSERT po operacji
SET IDENTITY_INSERT [tomasz1_voladmin].[Logs] OFF





-- Włącz IDENTITY_INSERT dla nowej tabeli
SET IDENTITY_INSERT [tomasz1_voladmin].[Seasons] ON

-- Wstaw dane ze starej tabeli do nowej tabeli
INSERT INTO [tomasz1_voladmin].[Seasons] (
[Id],
[Name]
)
SELECT
[idSezonu],
[nazwa]
FROM [tomasz1_ligasiatkowki].[dbo].[Sezon]

-- Wyłącz IDENTITY_INSERT po operacji
SET IDENTITY_INSERT [tomasz1_voladmin].[Seasons] OFF




-- Włącz IDENTITY_INSERT dla nowej tabeli
SET IDENTITY_INSERT [tomasz1_voladmin].[Rounds] ON

-- Wstaw dane ze starej tabeli do nowej tabeli
INSERT INTO [tomasz1_voladmin].[Rounds] (
[Id],
[Name],
[SeasonId]
)
SELECT
[idKolejki],
[nazwa],
[idSezonu]
FROM [tomasz1_ligasiatkowki].[dbo].[Kolejka]

-- Wyłącz IDENTITY_INSERT po operacji
SET IDENTITY_INSERT [tomasz1_voladmin].[Rounds] OFF




-- Włącz IDENTITY_INSERT dla nowej tabeli
SET IDENTITY_INSERT [tomasz1_voladmin].[SportsVenues] ON

-- Wstaw dane ze starej tabeli do nowej tabeli
INSERT INTO [tomasz1_voladmin].[SportsVenues] (
[Id],
[Name],
[AdditionalInfo]
)
SELECT
[idObiektu],
[nazwa],
[infoDodatkowe]
FROM [tomasz1_ligasiatkowki].[dbo].[ObiektSportowy]

-- Wyłącz IDENTITY_INSERT po operacji
SET IDENTITY_INSERT [tomasz1_voladmin].[SportsVenues] OFF








-- Włącz IDENTITY_INSERT dla nowej tabeli
SET IDENTITY_INSERT [tomasz1_voladmin].[Matches] ON

-- Wstaw dane ze starej tabeli do nowej tabeli
INSERT INTO [tomasz1_voladmin].[Matches] (
[Id],
[Schedule],
[CreationDate],
[VenueId],
[LeagueId],
[Sector],
[Team1Score],
[Team2Score],
[RoundId],
[RefereeId],
[UnknownRefereeName],
[MatchInfo],
[MvpId],
[Set1Team1Score],
[Set2Team1Score],
[Set3Team1Score],
[Set4Team1Score],
[Set5Team1Score],
[Set1Team2Score],
[Set2Team2Score],
[Set3Team2Score],
[Set4Team2Score],
[Set5Team2Score],
[MatchLeague],
[HomeTeamId],
[GuestTeamId]
)
SELECT
[idMeczu],
[termin],
[dataUtworzenia],
[idMiejsca],
[idLigi],
[sektor],
[wynikDruzyny1],
[wynikDruzyny2],
[idKolejki],
[idSedziego],
[nazwaNieznanegoSedziego],
[infoOMeczu],
[idMVP],
[w1s1],
[w1s2],
[w1s3],
[w1s4],
[w1s5],
[w2s1],
[w2s2],
[w2s3],
[w2s4],
[w2s5],
[ligaMeczu],
[idDruzyny1],
[idDruzyny2]
FROM [tomasz1_ligasiatkowki].[dbo].[Mecz]

-- Wyłącz IDENTITY_INSERT po operacji
SET IDENTITY_INSERT [tomasz1_voladmin].[Matches] OFF





-- Włącz IDENTITY_INSERT dla nowej tabeli
SET IDENTITY_INSERT [tomasz1_voladmin].[PersonalLogs] ON

-- Wstaw dane ze starej tabeli do nowej tabeli
INSERT INTO [tomasz1_voladmin].[PersonalLogs] (
[Id],
[UserId],
[LogId]
)
SELECT
[idPersonLogu],
[idUzytkownika],
[idLogu]
FROM [tomasz1_ligasiatkowki].[dbo].[LogPersonalny]

-- Wyłącz IDENTITY_INSERT po operacji
SET IDENTITY_INSERT [tomasz1_voladmin].[PersonalLogs] OFF




-- Włącz IDENTITY_INSERT dla nowej tabeli
SET IDENTITY_INSERT [tomasz1_voladmin].[TeamPlayers] ON

-- Wstaw dane ze starej tabeli do nowej tabeli
INSERT INTO [tomasz1_voladmin].[TeamPlayers] (
[Id],
[TeamId],
[PlayerId],
[JoinDate]
)
SELECT
[idDruzynyZawodnika],
[idDruzyny],
[idZawodnika],
[dataDolaczenia]
FROM [tomasz1_ligasiatkowki].[dbo].[DruzynaZwodnik]

-- Wyłącz IDENTITY_INSERT po operacji
SET IDENTITY_INSERT [tomasz1_voladmin].[TeamPlayers] OFF


-- Włącz IDENTITY_INSERT dla nowej tabeli
SET IDENTITY_INSERT [tomasz1_voladmin].[TypedResults] ON

-- Wstaw dane ze starej tabeli do nowej tabeli
INSERT INTO [tomasz1_voladmin].[TypedResults] (
[Id],
[UserId],
[MatchId],
[Score1],
[Score2]
)
SELECT
[idWynikuTypowanego],
[idUzytkownika],
[idMeczu],
[wynik1],
[wynik2]
FROM [tomasz1_ligasiatkowki].[dbo].[WynikTypowany]

-- Wyłącz IDENTITY_INSERT po operacji
SET IDENTITY_INSERT [tomasz1_voladmin].[TypedResults] OFF