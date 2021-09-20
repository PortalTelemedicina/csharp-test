CREATE TABLE [dbo].[Users](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](150) NOT NULL,
	[Email] [varchar](100) NULL,
	[Senha] [varchar](50) NOT NULL,
	[Perfil] [int] NOT NULL,
	[Created] [datetime] NOT NULL,
	[Modified] [datetime] NULL,
	[Last_login] [datetime] NULL,
 CONSTRAINT [Pk_Users] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]


CREATE TABLE [dbo].[Product](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NULL,
	[Descricao] [varchar](100) NULL,
	[Price] [decimal](18, 0) NULL,
	[Created] [datetime] NULL,
	[Modified] [datetime] NULL,
 CONSTRAINT [Pk_Product] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]

CREATE TABLE [dbo].[Orders](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ProductID] [bigint] NOT NULL,
	[UserID] [bigint] NOT NULL,
	[Price] [decimal](18, 0) NULL,
	[Quantidade] [int] NULL,
 CONSTRAINT [pk_order] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [fk_order_to_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ID])
GO

ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [fk_order_to_Product]
GO

ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [fk_order_to_User] FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([ID])
GO

ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [fk_order_to_User]
GO



--DROP TABLE Users
--DROP TABLE Product
--DROP TABLE Orders
