USE MinesweeperPlayers
GO
CREATE TABLE MinesweeperPlayers
(
	[Id] INT IDENTITY (1,1)  NOT NULL, 
    [FirstName] VARCHAR(50)  NOT NULL, 
    [LastName] VARCHAR(50)   NOT NULL, 
    [Sex] VARCHAR(50)        NOT NULL, 
    [Age] INT                NOT NULL, 
    [State] VARCHAR(50)      NOT NULL, 
    [Email] VARCHAR(50)      NOT NULL, 
    [Username] VARCHAR(50)   NOT NULL, 
    [Password] VARCHAR(50)   NOT NULL,
	PRIMARY KEY CLUSTERED ([Id] ASC)
)
GO
CREATE TABLE MinesweeperSavedGames
(
	[Id] INT IDENTITY (1,1)  NOT NULL, 
    [PlayerId] INT           NOT NULL,
    [Date] DATETIME          NOT NULL, 
    [SaveData] VARCHAR(MAX)  NOT NULL, 
	PRIMARY KEY CLUSTERED ([Id] ASC) 
)
