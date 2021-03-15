USE [master]
GO
/****** Object:  Database [Roulette_DB]    Script Date: 3/14/2021 10:02:04 PM ******/
CREATE DATABASE [Roulette_DB]
GO
ALTER DATABASE [Roulette_DB] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Roulette_DB].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Roulette_DB] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Roulette_DB] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Roulette_DB] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Roulette_DB] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Roulette_DB] SET ARITHABORT OFF 
GO
ALTER DATABASE [Roulette_DB] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Roulette_DB] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Roulette_DB] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Roulette_DB] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Roulette_DB] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Roulette_DB] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Roulette_DB] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Roulette_DB] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Roulette_DB] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Roulette_DB] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Roulette_DB] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Roulette_DB] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Roulette_DB] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Roulette_DB] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Roulette_DB] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Roulette_DB] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Roulette_DB] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Roulette_DB] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [Roulette_DB] SET  MULTI_USER 
GO
ALTER DATABASE [Roulette_DB] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Roulette_DB] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Roulette_DB] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Roulette_DB] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Roulette_DB] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Roulette_DB] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
ALTER DATABASE [Roulette_DB] SET QUERY_STORE = OFF
GO
USE [Roulette_DB]
GO
/****** Object:  Table [dbo].[bets]    Script Date: 3/14/2021 10:02:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[bets](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_roulette] [int] NOT NULL,
	[id_gambler] [int] NOT NULL,
	[number] [int] NULL,
	[color] [int] NOT NULL,
	[money_bet] [numeric](10, 2) NOT NULL,
	[date_bet] [datetime] NOT NULL,
	[date_play] [datetime] NULL,
	[status_winner] [int] NULL,
 CONSTRAINT [PK_bets] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[crupiers]    Script Date: 3/14/2021 10:02:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[crupiers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](50) NOT NULL,
	[access_key] [nvarchar](50) NOT NULL,
	[state] [bit] NOT NULL,
 CONSTRAINT [PK_crupiers] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[gamblers]    Script Date: 3/14/2021 10:02:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[gamblers](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](50) NOT NULL,
	[access_key] [nvarchar](max) NOT NULL,
	[credit] [numeric](10, 2) NOT NULL,
	[state] [bit] NOT NULL,
 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[roulettes]    Script Date: 3/14/2021 10:02:04 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[roulettes](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[id_crupier] [int] NOT NULL,
	[open_date] [datetime] NOT NULL,
	[number_winner] [int] NULL,
	[color_winner] [nvarchar](50) NULL,
	[close_date] [datetime] NULL,
	[state] [bit] NOT NULL,
 CONSTRAINT [PK_roulettes] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[roulettes]  WITH CHECK ADD  CONSTRAINT [FK_roulettes_crupier] FOREIGN KEY([id_crupier])
REFERENCES [dbo].[crupiers] ([id])
GO
ALTER TABLE [dbo].[roulettes] CHECK CONSTRAINT [FK_roulettes_crupier]
GO
USE [master]
GO
ALTER DATABASE [Roulette_DB] SET  READ_WRITE 
GO

USE [Roulette_DB]
GO

INSERT INTO [dbo].[crupiers] ([username] ,[access_key],[state]) VALUES ('Catalina', '93d36591-b06b-47c8-99c0-105aa735025f', 1)
INSERT INTO [dbo].[crupiers] ([username] ,[access_key],[state]) VALUES ('Jose', '8d9fddd4-cb48-48d7-aefb-b5e2da815325', 1)
GO

INSERT INTO [dbo].[gamblers] ([username],[access_key],[credit],[state]) VALUES ('Tucker_B', '60d71bfa-63f9-4b07-85e9-e9b22d828efe', 3340.00, 1);
INSERT INTO [dbo].[gamblers] ([username],[access_key],[credit],[state]) VALUES ('Mint_Ch', 'd3c62385-2b4e-449c-866a-6ade7358daa8', 10000, 1);
GO

INSERT INTO [dbo].[roulettes] ([id_crupier],[open_date], [state]) VALUES (1, GETDATE(), 1 )
INSERT INTO [dbo].[roulettes] ([id_crupier],[open_date], [state]) VALUES (1, GETDATE(), 1 )
GO

INSERT INTO [dbo].[bets]([id_roulette],[id_gambler],[number],[color],[money_bet],[date_bet]) VALUES (2, 1, 11, 1, 100, GETDATE())
INSERT INTO [dbo].[bets]([id_roulette],[id_gambler],[number],[color],[money_bet],[date_bet]) VALUES (1, 1, 10, 1, 100, GETDATE())
INSERT INTO [dbo].[bets]([id_roulette],[id_gambler],[number],[color],[money_bet],[date_bet]) VALUES (1, 1, NULL, 0, 100, GETDATE())
INSERT INTO [dbo].[bets]([id_roulette],[id_gambler],[number],[color],[money_bet],[date_bet]) VALUES (2, 1, 11, 1, 100, GETDATE())

GO
