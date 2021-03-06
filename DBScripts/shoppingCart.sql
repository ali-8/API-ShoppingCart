USE [master]
GO
/****** Object:  Database [ShoppingCart]    Script Date: 30-05-2022 00:13:59 ******/
CREATE DATABASE [ShoppingCart]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ShoppingCart', FILENAME = N'E:\Programs\SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\ShoppingCart.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'ShoppingCart_log', FILENAME = N'E:\Programs\SQL Server\MSSQL14.MSSQLSERVER\MSSQL\DATA\ShoppingCart_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
GO
ALTER DATABASE [ShoppingCart] SET COMPATIBILITY_LEVEL = 140
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ShoppingCart].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ShoppingCart] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ShoppingCart] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ShoppingCart] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ShoppingCart] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ShoppingCart] SET ARITHABORT OFF 
GO
ALTER DATABASE [ShoppingCart] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [ShoppingCart] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ShoppingCart] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ShoppingCart] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ShoppingCart] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ShoppingCart] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ShoppingCart] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ShoppingCart] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ShoppingCart] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ShoppingCart] SET  DISABLE_BROKER 
GO
ALTER DATABASE [ShoppingCart] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ShoppingCart] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ShoppingCart] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ShoppingCart] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ShoppingCart] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ShoppingCart] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [ShoppingCart] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ShoppingCart] SET RECOVERY FULL 
GO
ALTER DATABASE [ShoppingCart] SET  MULTI_USER 
GO
ALTER DATABASE [ShoppingCart] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ShoppingCart] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ShoppingCart] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ShoppingCart] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [ShoppingCart] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'ShoppingCart', N'ON'
GO
ALTER DATABASE [ShoppingCart] SET QUERY_STORE = OFF
GO
USE [ShoppingCart]
GO
/****** Object:  Table [dbo].[tblCart]    Script Date: 30-05-2022 00:14:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCart](
	[CartID] [int] IDENTITY(1,1) NOT NULL,
	[FK_ProductID] [int] NOT NULL,
	[ProductQuantity] [int] NOT NULL,
	[CostPerUnit] [decimal](8, 2) NOT NULL,
	[NormalDayDiscount] [decimal](8, 2) NULL,
	[Add_WeekendsDiscount] [decimal](8, 2) NULL,
	[TotalDiscount] [decimal](8, 2) NOT NULL,
	[DeliveryCharges] [decimal](8, 2) NOT NULL,
	[TotalCost] [decimal](8, 2) NOT NULL,
 CONSTRAINT [PK_CartID] PRIMARY KEY CLUSTERED 
(
	[CartID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblDeliveryCharges]    Script Date: 30-05-2022 00:14:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDeliveryCharges](
	[DeliveryChargesID] [int] IDENTITY(1,1) NOT NULL,
	[FK_ProductcategoryID] [int] NOT NULL,
	[DeliveryCharges] [float] NOT NULL,
 CONSTRAINT [PK_DeliveryChargesID] PRIMARY KEY CLUSTERED 
(
	[DeliveryChargesID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblDiscount]    Script Date: 30-05-2022 00:14:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDiscount](
	[DiscountID] [int] IDENTITY(1,1) NOT NULL,
	[FK_ProductID] [int] NOT NULL,
	[NormalDayDiscount] [float] NOT NULL,
	[Add_WeekendsDiscount] [float] NOT NULL,
 CONSTRAINT [PK_DiscountID] PRIMARY KEY CLUSTERED 
(
	[DiscountID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblProductCategory]    Script Date: 30-05-2022 00:14:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblProductCategory](
	[ProductcategoryID] [int] IDENTITY(1,1) NOT NULL,
	[ProductCategoryName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_ProductcategoryID] PRIMARY KEY CLUSTERED 
(
	[ProductcategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblProductCostTable]    Script Date: 30-05-2022 00:14:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblProductCostTable](
	[ProductCostID] [int] IDENTITY(1,1) NOT NULL,
	[FK_ProductID] [int] NOT NULL,
	[ProductCost] [int] NOT NULL,
 CONSTRAINT [PK_ProductCostID] PRIMARY KEY CLUSTERED 
(
	[ProductCostID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[tblProducts]    Script Date: 30-05-2022 00:14:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblProducts](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[FK_ProductcategoryID] [int] NOT NULL,
	[ProductName] [varchar](100) NOT NULL,
 CONSTRAINT [PK_ProductID] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[tblCart]  WITH CHECK ADD FOREIGN KEY([FK_ProductID])
REFERENCES [dbo].[tblProducts] ([ProductID])
GO
ALTER TABLE [dbo].[tblDeliveryCharges]  WITH CHECK ADD FOREIGN KEY([FK_ProductcategoryID])
REFERENCES [dbo].[tblProductCategory] ([ProductcategoryID])
GO
ALTER TABLE [dbo].[tblDiscount]  WITH CHECK ADD FOREIGN KEY([FK_ProductID])
REFERENCES [dbo].[tblProducts] ([ProductID])
GO
ALTER TABLE [dbo].[tblProductCostTable]  WITH CHECK ADD FOREIGN KEY([FK_ProductID])
REFERENCES [dbo].[tblProducts] ([ProductID])
GO
ALTER TABLE [dbo].[tblProducts]  WITH CHECK ADD FOREIGN KEY([FK_ProductcategoryID])
REFERENCES [dbo].[tblProductCategory] ([ProductcategoryID])
GO
USE [master]
GO
ALTER DATABASE [ShoppingCart] SET  READ_WRITE 
GO
