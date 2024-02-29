 
/****** Object:  Table [dbo].[Artykul]    Script Date: 2023-05-28 21:25:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Artykul](
	[idArtykulu] [int] IDENTITY(1,1) NOT NULL,
	[idAutora] [int] NOT NULL,
	[tresc] [text] NOT NULL,
	[dataUtworzenia] [datetime] NOT NULL,
	[tytul] [varchar](255) NOT NULL,
	[aktywny] [bit] NOT NULL,
	[zdjecie] [image] NOT NULL,
 CONSTRAINT [PK_Artykul] PRIMARY KEY CLUSTERED 
(
	[idArtykulu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Druzyna]    Script Date: 2023-05-28 21:25:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Druzyna](
	[idDruzyny] [int] IDENTITY(1,1) NOT NULL,
	[nazwa] [varchar](255) NOT NULL,
	[dataUtworzenia] [datetime] NOT NULL,
	[zdjecie] [image] NULL,
	[idLigi] [int] NOT NULL,
	[zaakceptowana] [bit] NOT NULL,
	[idKapitana] [int] NOT NULL,
	[email] [varchar](255) NOT NULL,
	[zgloszona] [bit] NOT NULL,
	[ileZmian] [int] NOT NULL,
	[zdjecieWidth] [int] NULL,
	[zdjecieHeight] [int] NULL,
	[logo] [image] NULL,
	[logoWidth] [int] NULL,
	[logoHeight] [int] NULL,
	[telefon] [varchar](255) NOT NULL,
	[opisDruzyny] [varchar](5000) NULL,
	[stronaWWW] [varchar](255) NULL,
	[korekcjaPunktow] [int] NULL,
 CONSTRAINT [PK_Druzyna] PRIMARY KEY CLUSTERED 
(
	[idDruzyny] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DruzynaZwodnik]    Script Date: 2023-05-28 21:25:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DruzynaZwodnik](
	[idDruzynyZawodnika] [int] IDENTITY(1,1) NOT NULL,
	[idDruzyny] [int] NOT NULL,
	[idZawodnika] [int] NOT NULL,
	[dataDolaczenia] [datetime] NOT NULL,
 CONSTRAINT [PK_DruzynaZwodnik] PRIMARY KEY CLUSTERED 
(
	[idDruzynyZawodnika] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Kategoria]    Script Date: 2023-05-28 21:25:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Kategoria](
	[idKategorii] [int] IDENTITY(1,1) NOT NULL,
	[nazwa] [varchar](255) NOT NULL,
 CONSTRAINT [PK_Kategoria] PRIMARY KEY CLUSTERED 
(
	[idKategorii] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Kolejka]    Script Date: 2023-05-28 21:25:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Kolejka](
	[idKolejki] [int] IDENTITY(1,1) NOT NULL,
	[nazwa] [varchar](255) NOT NULL,
	[idSezonu] [int] NOT NULL,
 CONSTRAINT [PK_Kolejka] PRIMARY KEY CLUSTERED 
(
	[idKolejki] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Komentarz]    Script Date: 2023-05-28 21:25:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Komentarz](
	[idKomentarza] [int] IDENTITY(1,1) NOT NULL,
	[idTresci] [int] NOT NULL,
	[idAutora] [int] NOT NULL,
	[tresc] [varchar](3000) NOT NULL,
	[aktywny] [bit] NOT NULL,
	[dataUtworzenia] [datetime] NOT NULL,
	[idMiejscaKomentarza] [int] NOT NULL,
 CONSTRAINT [PK_Komentarz] PRIMARY KEY CLUSTERED 
(
	[idKomentarza] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Liga]    Script Date: 2023-05-28 21:25:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Liga](
	[idLigi] [int] IDENTITY(1,1) NOT NULL,
	[nazwa] [varchar](255) NOT NULL,
 CONSTRAINT [PK_Liga] PRIMARY KEY CLUSTERED 
(
	[idLigi] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Log]    Script Date: 2023-05-28 21:25:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Log](
	[idLogu] [int] IDENTITY(1,1) NOT NULL,
	[link] [varchar](255) NULL,
	[opis] [varchar](255) NOT NULL,
	[data] [datetime] NOT NULL,
	[admin] [bit] NOT NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED 
(
	[idLogu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LogPersonalny]    Script Date: 2023-05-28 21:25:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LogPersonalny](
	[idPersonLogu] [int] IDENTITY(1,1) NOT NULL,
	[idUzytkownika] [int] NOT NULL,
	[idLogu] [int] NOT NULL,
 CONSTRAINT [PK_LogPersonalny] PRIMARY KEY CLUSTERED 
(
	[idPersonLogu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Mecz]    Script Date: 2023-05-28 21:25:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Mecz](
	[idMeczu] [int] IDENTITY(1,1) NOT NULL,
	[termin] [datetime] NOT NULL,
	[dataUtworzenia] [datetime] NOT NULL,
	[idMiejsca] [int] NOT NULL,
	[idLigi] [int] NOT NULL,
	[sektor] [tinyint] NOT NULL,
	[idDruzyny1] [int] NOT NULL,
	[idDruzyny2] [int] NOT NULL,
	[wynikDruzyny1] [tinyint] NOT NULL,
	[wynikDruzyny2] [tinyint] NOT NULL,
	[idKolejki] [int] NOT NULL,
	[idSedziego] [int] NULL,
	[nazwaNieznanegoSedziego] [varchar](255) NULL,
	[infoOMeczu] [varchar](3000) NULL,
	[idMVP] [int] NOT NULL,
	[w1s1] [tinyint] NULL,
	[w1s2] [tinyint] NULL,
	[w1s3] [tinyint] NULL,
	[w1s4] [tinyint] NULL,
	[w1s5] [tinyint] NULL,
	[w2s1] [tinyint] NULL,
	[w2s2] [tinyint] NULL,
	[w2s3] [tinyint] NULL,
	[w2s4] [tinyint] NULL,
	[w2s5] [tinyint] NULL,
	[ligaMeczu] [varchar](255) NULL,
 CONSTRAINT [PK_Mecz] PRIMARY KEY CLUSTERED 
(
	[idMeczu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MiejsceKomentarza]    Script Date: 2023-05-28 21:25:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MiejsceKomentarza](
	[idMiejscaKomentarza] [int] IDENTITY(1,1) NOT NULL,
	[nazwa] [varchar](50) NOT NULL,
 CONSTRAINT [PK_MiejsceKomentarza] PRIMARY KEY CLUSTERED 
(
	[idMiejscaKomentarza] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ObiektSportowy]    Script Date: 2023-05-28 21:25:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ObiektSportowy](
	[idObiektu] [int] IDENTITY(1,1) NOT NULL,
	[nazwa] [varchar](255) NOT NULL,
	[infoDodatkowe] [varchar](3000) NULL,
 CONSTRAINT [PK_ObiektSportowy] PRIMARY KEY CLUSTERED 
(
	[idObiektu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Pozycja]    Script Date: 2023-05-28 21:25:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Pozycja](
	[idPozycji] [int] IDENTITY(1,1) NOT NULL,
	[nazwa] [varchar](255) NOT NULL,
 CONSTRAINT [PK_Pozycja] PRIMARY KEY CLUSTERED 
(
	[idPozycji] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Program]    Script Date: 2023-05-28 21:25:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Program](
	[idWersji] [int] IDENTITY(1,1) NOT NULL,
	[wersja] [int] NOT NULL,
 CONSTRAINT [PK_Program] PRIMARY KEY CLUSTERED 
(
	[idWersji] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Sezon]    Script Date: 2023-05-28 21:25:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sezon](
	[idSezonu] [int] IDENTITY(1,1) NOT NULL,
	[nazwa] [varchar](255) NOT NULL,
 CONSTRAINT [PK_Sezon] PRIMARY KEY CLUSTERED 
(
	[idSezonu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[StatystykiDruzyny]    Script Date: 2023-05-28 21:25:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[StatystykiDruzyny](
	[idDruzyny] [int] NOT NULL,
	[punkty] [int] NOT NULL,
	[rozegraneMecze] [int] NOT NULL,
	[wygraneMecze] [int] NOT NULL,
	[przegraneMecze] [int] NOT NULL,
	[wygrane32] [int] NOT NULL,
	[przegrane23] [int] NOT NULL,
	[miejsceGlobalne] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[idDruzyny] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Temat]    Script Date: 2023-05-28 21:25:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Temat](
	[idTematu] [int] IDENTITY(1,1) NOT NULL,
	[idAutora] [int] NOT NULL,
	[tresc] [varchar](3000) NOT NULL,
	[dataUtworzenia] [datetime2](7) NOT NULL,
	[tytul] [varchar](255) NOT NULL,
	[idKategorii] [int] NOT NULL,
	[aktywny] [bit] NOT NULL,
 CONSTRAINT [PK_Temat] PRIMARY KEY CLUSTERED 
(
	[idTematu] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Uzytkownik]    Script Date: 2023-05-28 21:25:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Uzytkownik](
	[idUzytkownika] [int] IDENTITY(1,1) NOT NULL,
	[imie] [varchar](255) NOT NULL,
	[nazwisko] [varchar](255) NOT NULL,
	[idKonta] [uniqueidentifier] NULL,
	[rokUrodzenia] [int] NULL,
	[miasto] [varchar](255) NULL,
	[infoOSobie] [varchar](3000) NULL,
	[zdjecie] [image] NULL,
	[plec] [bit] NOT NULL,
	[wzrost] [tinyint] NULL,
	[waga] [tinyint] NULL,
	[nrNaKoszulce] [tinyint] NULL,
	[zasiegBlok] [int] NULL,
	[zasiegAtak] [int] NULL,
	[siatkarskiIdol] [varchar](255) NULL,
	[emailDodatkowy] [varchar](255) NULL,
	[hobby] [varchar](255) NULL,
	[telefon] [varchar](255) NULL,
	[idPozycji] [int] NOT NULL,
	[zdjecieWidth] [int] NULL,
	[zdjecieHeight] [int] NULL,
 CONSTRAINT [PK_Uzytkownik] PRIMARY KEY CLUSTERED 
(
	[idUzytkownika] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[WynikTypowany]    Script Date: 2023-05-28 21:25:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[WynikTypowany](
	[idWynikuTypowanego] [int] IDENTITY(1,1) NOT NULL,
	[idUzytkownika] [int] NOT NULL,
	[idMeczu] [int] NOT NULL,
	[wynik1] [tinyint] NOT NULL,
	[wynik2] [tinyint] NOT NULL,
 CONSTRAINT [PK_WynikTypowany] PRIMARY KEY CLUSTERED 
(
	[idWynikuTypowanego] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Zaproszenie]    Script Date: 2023-05-28 21:25:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Zaproszenie](
	[idZaproszenia] [int] IDENTITY(1,1) NOT NULL,
	[idZawodnika] [int] NOT NULL,
	[idDruzyny] [int] NOT NULL,
	[dataUtworzenia] [datetime] NOT NULL,
 CONSTRAINT [PK_Zaproszenie] PRIMARY KEY CLUSTERED 
(
	[idZaproszenia] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Artykul] ADD  CONSTRAINT [DF_Artykul_dataUtworzenia]  DEFAULT (getdate()) FOR [dataUtworzenia]
GO
ALTER TABLE [dbo].[Artykul] ADD  CONSTRAINT [DF_Artykul_aktywny]  DEFAULT ((1)) FOR [aktywny]
GO
ALTER TABLE [dbo].[Druzyna] ADD  CONSTRAINT [DF_Druzyna_zgloszona]  DEFAULT ((1)) FOR [zgloszona]
GO
ALTER TABLE [dbo].[Druzyna] ADD  CONSTRAINT [DF_Druzyna_ileZmian]  DEFAULT ((0)) FOR [ileZmian]
GO
ALTER TABLE [dbo].[Komentarz] ADD  CONSTRAINT [DF_Komentarz_idMiejscaKomentarza]  DEFAULT ((1)) FOR [idMiejscaKomentarza]
GO
ALTER TABLE [dbo].[Mecz] ADD  CONSTRAINT [DF_Mecz_idMVP]  DEFAULT ((39)) FOR [idMVP]
GO
ALTER TABLE [dbo].[Temat] ADD  DEFAULT ((1)) FOR [aktywny]
GO
ALTER TABLE [dbo].[Artykul]  WITH CHECK ADD  CONSTRAINT [FK_Artykul_Uzytkownik] FOREIGN KEY([idAutora])
REFERENCES [dbo].[Uzytkownik] ([idUzytkownika])
GO
ALTER TABLE [dbo].[Artykul] CHECK CONSTRAINT [FK_Artykul_Uzytkownik]
GO
ALTER TABLE [dbo].[Druzyna]  WITH CHECK ADD  CONSTRAINT [FK_Druzyna_Liga] FOREIGN KEY([idLigi])
REFERENCES [dbo].[Liga] ([idLigi])
GO
ALTER TABLE [dbo].[Druzyna] CHECK CONSTRAINT [FK_Druzyna_Liga]
GO
ALTER TABLE [dbo].[Druzyna]  WITH CHECK ADD  CONSTRAINT [FK_Druzyna_Uzytkownik] FOREIGN KEY([idKapitana])
REFERENCES [dbo].[Uzytkownik] ([idUzytkownika])
GO
ALTER TABLE [dbo].[Druzyna] CHECK CONSTRAINT [FK_Druzyna_Uzytkownik]
GO
ALTER TABLE [dbo].[DruzynaZwodnik]  WITH CHECK ADD  CONSTRAINT [FK_DruzynaZwodnik_Druzyna] FOREIGN KEY([idDruzyny])
REFERENCES [dbo].[Druzyna] ([idDruzyny])
GO
ALTER TABLE [dbo].[DruzynaZwodnik] CHECK CONSTRAINT [FK_DruzynaZwodnik_Druzyna]
GO
ALTER TABLE [dbo].[DruzynaZwodnik]  WITH CHECK ADD  CONSTRAINT [FK_DruzynaZwodnik_Uzytkownik] FOREIGN KEY([idZawodnika])
REFERENCES [dbo].[Uzytkownik] ([idUzytkownika])
GO
ALTER TABLE [dbo].[DruzynaZwodnik] CHECK CONSTRAINT [FK_DruzynaZwodnik_Uzytkownik]
GO
ALTER TABLE [dbo].[Kolejka]  WITH CHECK ADD  CONSTRAINT [FK_Kolejka_Sezon] FOREIGN KEY([idSezonu])
REFERENCES [dbo].[Sezon] ([idSezonu])
GO
ALTER TABLE [dbo].[Kolejka] CHECK CONSTRAINT [FK_Kolejka_Sezon]
GO
ALTER TABLE [dbo].[Komentarz]  WITH CHECK ADD  CONSTRAINT [FK_Komentarz_MiejsceKomentarza] FOREIGN KEY([idMiejscaKomentarza])
REFERENCES [dbo].[MiejsceKomentarza] ([idMiejscaKomentarza])
GO
ALTER TABLE [dbo].[Komentarz] CHECK CONSTRAINT [FK_Komentarz_MiejsceKomentarza]
GO
ALTER TABLE [dbo].[Komentarz]  WITH CHECK ADD  CONSTRAINT [FK_Komentarz_Uzytkownik] FOREIGN KEY([idAutora])
REFERENCES [dbo].[Uzytkownik] ([idUzytkownika])
GO
ALTER TABLE [dbo].[Komentarz] CHECK CONSTRAINT [FK_Komentarz_Uzytkownik]
GO
ALTER TABLE [dbo].[LogPersonalny]  WITH CHECK ADD  CONSTRAINT [FK_LogPersonalny_Log] FOREIGN KEY([idLogu])
REFERENCES [dbo].[Log] ([idLogu])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LogPersonalny] CHECK CONSTRAINT [FK_LogPersonalny_Log]
GO
ALTER TABLE [dbo].[LogPersonalny]  WITH CHECK ADD  CONSTRAINT [FK_LogPersonalny_Uzytkownik] FOREIGN KEY([idUzytkownika])
REFERENCES [dbo].[Uzytkownik] ([idUzytkownika])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[LogPersonalny] CHECK CONSTRAINT [FK_LogPersonalny_Uzytkownik]
GO
ALTER TABLE [dbo].[Mecz]  WITH CHECK ADD  CONSTRAINT [FK_Mecz_Druzyna] FOREIGN KEY([idDruzyny1])
REFERENCES [dbo].[Druzyna] ([idDruzyny])
GO
ALTER TABLE [dbo].[Mecz] CHECK CONSTRAINT [FK_Mecz_Druzyna]
GO
ALTER TABLE [dbo].[Mecz]  WITH CHECK ADD  CONSTRAINT [FK_Mecz_Druzyna1] FOREIGN KEY([idDruzyny2])
REFERENCES [dbo].[Druzyna] ([idDruzyny])
GO
ALTER TABLE [dbo].[Mecz] CHECK CONSTRAINT [FK_Mecz_Druzyna1]
GO
ALTER TABLE [dbo].[Mecz]  WITH CHECK ADD  CONSTRAINT [FK_Mecz_Kolejka] FOREIGN KEY([idKolejki])
REFERENCES [dbo].[Kolejka] ([idKolejki])
GO
ALTER TABLE [dbo].[Mecz] CHECK CONSTRAINT [FK_Mecz_Kolejka]
GO
ALTER TABLE [dbo].[Mecz]  WITH CHECK ADD  CONSTRAINT [FK_Mecz_Liga] FOREIGN KEY([idLigi])
REFERENCES [dbo].[Liga] ([idLigi])
GO
ALTER TABLE [dbo].[Mecz] CHECK CONSTRAINT [FK_Mecz_Liga]
GO
ALTER TABLE [dbo].[Mecz]  WITH CHECK ADD  CONSTRAINT [FK_Mecz_ObiektSportowy] FOREIGN KEY([idMiejsca])
REFERENCES [dbo].[ObiektSportowy] ([idObiektu])
GO
ALTER TABLE [dbo].[Mecz] CHECK CONSTRAINT [FK_Mecz_ObiektSportowy]
GO
ALTER TABLE [dbo].[Mecz]  WITH CHECK ADD  CONSTRAINT [FK_Mecz_Uzytkownik] FOREIGN KEY([idSedziego])
REFERENCES [dbo].[Uzytkownik] ([idUzytkownika])
GO
ALTER TABLE [dbo].[Mecz] CHECK CONSTRAINT [FK_Mecz_Uzytkownik]
GO
ALTER TABLE [dbo].[Mecz]  WITH CHECK ADD  CONSTRAINT [FK_Mecz_Uzytkownik1] FOREIGN KEY([idMVP])
REFERENCES [dbo].[Uzytkownik] ([idUzytkownika])
GO
ALTER TABLE [dbo].[Mecz] CHECK CONSTRAINT [FK_Mecz_Uzytkownik1]
GO
ALTER TABLE [dbo].[StatystykiDruzyny]  WITH CHECK ADD  CONSTRAINT [FK_StatystykiDruzyny_Druzyna] FOREIGN KEY([idDruzyny])
REFERENCES [dbo].[Druzyna] ([idDruzyny])
GO
ALTER TABLE [dbo].[StatystykiDruzyny] CHECK CONSTRAINT [FK_StatystykiDruzyny_Druzyna]
GO
ALTER TABLE [dbo].[Temat]  WITH CHECK ADD  CONSTRAINT [FK_Temat_Kategoria] FOREIGN KEY([idKategorii])
REFERENCES [dbo].[Kategoria] ([idKategorii])
GO
ALTER TABLE [dbo].[Temat] CHECK CONSTRAINT [FK_Temat_Kategoria]
GO
ALTER TABLE [dbo].[Temat]  WITH CHECK ADD  CONSTRAINT [FK_Temat_Uzytkownik] FOREIGN KEY([idAutora])
REFERENCES [dbo].[Uzytkownik] ([idUzytkownika])
GO
ALTER TABLE [dbo].[Temat] CHECK CONSTRAINT [FK_Temat_Uzytkownik]
GO
ALTER TABLE [dbo].[Uzytkownik]  WITH CHECK ADD  CONSTRAINT [FK_Uzytkownik_aspnet_Users] FOREIGN KEY([idKonta])
REFERENCES [dbo].[aspnet_Users] ([UserId])
GO
ALTER TABLE [dbo].[Uzytkownik] CHECK CONSTRAINT [FK_Uzytkownik_aspnet_Users]
GO
ALTER TABLE [dbo].[Uzytkownik]  WITH CHECK ADD  CONSTRAINT [FK_Uzytkownik_Pozycja] FOREIGN KEY([idPozycji])
REFERENCES [dbo].[Pozycja] ([idPozycji])
GO
ALTER TABLE [dbo].[Uzytkownik] CHECK CONSTRAINT [FK_Uzytkownik_Pozycja]
GO
ALTER TABLE [dbo].[WynikTypowany]  WITH CHECK ADD  CONSTRAINT [FK_WynikTypowany_Mecz] FOREIGN KEY([idMeczu])
REFERENCES [dbo].[Mecz] ([idMeczu])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[WynikTypowany] CHECK CONSTRAINT [FK_WynikTypowany_Mecz]
GO
ALTER TABLE [dbo].[WynikTypowany]  WITH CHECK ADD  CONSTRAINT [FK_WynikTypowany_Uzytkownik] FOREIGN KEY([idUzytkownika])
REFERENCES [dbo].[Uzytkownik] ([idUzytkownika])
GO
ALTER TABLE [dbo].[WynikTypowany] CHECK CONSTRAINT [FK_WynikTypowany_Uzytkownik]
GO
ALTER TABLE [dbo].[Zaproszenie]  WITH CHECK ADD  CONSTRAINT [FK_Zaproszenie_Druzyna] FOREIGN KEY([idDruzyny])
REFERENCES [dbo].[Druzyna] ([idDruzyny])
GO
ALTER TABLE [dbo].[Zaproszenie] CHECK CONSTRAINT [FK_Zaproszenie_Druzyna]
GO
ALTER TABLE [dbo].[Zaproszenie]  WITH CHECK ADD  CONSTRAINT [FK_Zaproszenie_Uzytkownik] FOREIGN KEY([idZawodnika])
REFERENCES [dbo].[Uzytkownik] ([idUzytkownika])
GO
ALTER TABLE [dbo].[Zaproszenie] CHECK CONSTRAINT [FK_Zaproszenie_Uzytkownik]
GO
