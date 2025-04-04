USE [master]
GO
/****** Object:  Database [Book_Api_Management]    Script Date: 31-Mar-25 2:50:00 PM ******/
CREATE DATABASE [Book_Api_Management]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'Book_Api_Management', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Book_Api_Management.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'Book_Api_Management_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER\MSSQL\DATA\Book_Api_Management_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [Book_Api_Management] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Book_Api_Management].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Book_Api_Management] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Book_Api_Management] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Book_Api_Management] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Book_Api_Management] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Book_Api_Management] SET ARITHABORT OFF 
GO
ALTER DATABASE [Book_Api_Management] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Book_Api_Management] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Book_Api_Management] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Book_Api_Management] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Book_Api_Management] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Book_Api_Management] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Book_Api_Management] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Book_Api_Management] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Book_Api_Management] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Book_Api_Management] SET  ENABLE_BROKER 
GO
ALTER DATABASE [Book_Api_Management] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Book_Api_Management] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Book_Api_Management] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Book_Api_Management] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Book_Api_Management] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Book_Api_Management] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Book_Api_Management] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Book_Api_Management] SET RECOVERY FULL 
GO
ALTER DATABASE [Book_Api_Management] SET  MULTI_USER 
GO
ALTER DATABASE [Book_Api_Management] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Book_Api_Management] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Book_Api_Management] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Book_Api_Management] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Book_Api_Management] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [Book_Api_Management] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'Book_Api_Management', N'ON'
GO
ALTER DATABASE [Book_Api_Management] SET QUERY_STORE = ON
GO
ALTER DATABASE [Book_Api_Management] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [Book_Api_Management]
GO
/****** Object:  Table [dbo].[Book]    Script Date: 31-Mar-25 2:50:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Book](
	[isbn] [char](13) NOT NULL,
	[title] [nvarchar](200) NOT NULL,
	[category_id] [smallint] NOT NULL,
	[author] [nvarchar](100) NOT NULL,
	[year_of_publication] [smallint] NOT NULL,
	[quantity] [int] NOT NULL,
	[price] [decimal](8, 0) NOT NULL,
	[is_discontinued] [bit] NOT NULL,
 CONSTRAINT [PK_Book] PRIMARY KEY CLUSTERED 
(
	[isbn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Category]    Script Date: 31-Mar-25 2:50:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Category](
	[category_id] [smallint] IDENTITY(1,1) NOT NULL,
	[category_name] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Category] PRIMARY KEY CLUSTERED 
(
	[category_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Customer]    Script Date: 31-Mar-25 2:50:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Customer](
	[customer_id] [int] IDENTITY(1,1) NOT NULL,
	[customer_name] [nvarchar](100) NOT NULL,
	[customer_phone] [char](10) NOT NULL,
	[customer_gender] [bit] NOT NULL,
 CONSTRAINT [PK_Customer] PRIMARY KEY CLUSTERED 
(
	[customer_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Order]    Script Date: 31-Mar-25 2:50:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Order](
	[order_id] [bigint] IDENTITY(1,1) NOT NULL,
	[staff_id] [int] NOT NULL,
	[promotion_id] [smallint] NULL,
	[customer_id] [int] NOT NULL,
	[created_time] [datetime2](7) NOT NULL,
	[status] [smallint] NOT NULL,
	[note] [nvarchar](max) NULL,
	[total_price] [decimal](11, 3) NOT NULL,
 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[order_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetail]    Script Date: 31-Mar-25 2:50:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[OrderDetail](
	[order_id] [bigint] NOT NULL,
	[isbn] [char](13) NOT NULL,
	[quantity] [smallint] NOT NULL,
	[price] [decimal](8, 0) NOT NULL,
 CONSTRAINT [PK_OrderDetail] PRIMARY KEY CLUSTERED 
(
	[order_id] ASC,
	[isbn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Promotion]    Script Date: 31-Mar-25 2:50:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Promotion](
	[promotion_id] [smallint] IDENTITY(1,1) NOT NULL,
	[promotion_name] [nvarchar](100) NOT NULL,
	[start_date] [date] NOT NULL,
	[end_date] [date] NOT NULL,
	[condition] [decimal](8, 0) NOT NULL,
	[discount_percent] [decimal](3, 2) NOT NULL,
	[quantity] [smallint] NOT NULL,
 CONSTRAINT [PK_Promotion] PRIMARY KEY CLUSTERED 
(
	[promotion_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Staff]    Script Date: 31-Mar-25 2:50:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Staff](
	[staff_id] [int] IDENTITY(1,1) NOT NULL,
	[email] [varchar](50) NOT NULL,
	[fullname] [nvarchar](100) NOT NULL,
	[phone] [char](10) NOT NULL,
	[hash_pwd] [varchar](255) NOT NULL,
	[gender] [bit] NOT NULL,
	[is_active] [bit] NOT NULL,
	[is_ban] [bit] NOT NULL,
 CONSTRAINT [PK_Staff] PRIMARY KEY CLUSTERED 
(
	[staff_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
INSERT [dbo].[Book] ([isbn], [title], [category_id], [author], [year_of_publication], [quantity], [price], [is_discontinued]) VALUES (N'8934974148715', N'Nguyễn Nhật Ánh Trong Mắt Đồng Nghiệp', 4, N'Nguyễn Nhật Ánh', 2017, 70, CAST(135000 AS Decimal(8, 0)), 0)
INSERT [dbo].[Book] ([isbn], [title], [category_id], [author], [year_of_publication], [quantity], [price], [is_discontinued]) VALUES (N'8934974151647', N'Cây Chuối Non Đi Giày Xanh (Bìa Cứng)
', 4, N'Nguyễn Nhật Ánh', 2018, 35, CAST(220000 AS Decimal(8, 0)), 0)
INSERT [dbo].[Book] ([isbn], [title], [category_id], [author], [year_of_publication], [quantity], [price], [is_discontinued]) VALUES (N'8935088549429', N'Vợ Nhặt', 8, N'Kim Lân', 1990, 56, CAST(49000 AS Decimal(8, 0)), 0)
INSERT [dbo].[Book] ([isbn], [title], [category_id], [author], [year_of_publication], [quantity], [price], [is_discontinued]) VALUES (N'8935236427838', N'Nam Cao - Lão Hạc (Tái Bản)', 8, N'Nam Cao', 2000, 58, CAST(100000 AS Decimal(8, 0)), 0)
INSERT [dbo].[Book] ([isbn], [title], [category_id], [author], [year_of_publication], [quantity], [price], [is_discontinued]) VALUES (N'8935236429177', N'Lều Chõng (Tái Bản 2023)', 8, N'Ngô Tất Tố', 2023, 86, CAST(98000 AS Decimal(8, 0)), 0)
GO
SET IDENTITY_INSERT [dbo].[Category] ON 

INSERT [dbo].[Category] ([category_id], [category_name]) VALUES (5, N'Kỹ Năng Sống')
INSERT [dbo].[Category] ([category_id], [category_name]) VALUES (4, N'Tiểu Thuyết')
INSERT [dbo].[Category] ([category_id], [category_name]) VALUES (1, N'Truyện Tranh')
INSERT [dbo].[Category] ([category_id], [category_name]) VALUES (8, N'Văn Học')
INSERT [dbo].[Category] ([category_id], [category_name]) VALUES (6, N'Viễn Tưởng')
SET IDENTITY_INSERT [dbo].[Category] OFF
GO
SET IDENTITY_INSERT [dbo].[Customer] ON 

INSERT [dbo].[Customer] ([customer_id], [customer_name], [customer_phone], [customer_gender]) VALUES (2, N'Nguyễn Ngọc Thiên Phúc', N'0123456781', 0)
SET IDENTITY_INSERT [dbo].[Customer] OFF
GO
SET IDENTITY_INSERT [dbo].[Order] ON 

INSERT [dbo].[Order] ([order_id], [staff_id], [promotion_id], [customer_id], [created_time], [status], [note], [total_price]) VALUES (3, 7, 2, 2, CAST(N'2025-03-31T13:57:53.0752618' AS DateTime2), 2, N'Nhân viên đưa thiếu sách
➡ Đã bổ sung sách cho khách hàng', CAST(2082500.000 AS Decimal(11, 3)))
SET IDENTITY_INSERT [dbo].[Order] OFF
GO
INSERT [dbo].[OrderDetail] ([order_id], [isbn], [quantity], [price]) VALUES (3, N'8934974148715', 10, CAST(135000 AS Decimal(8, 0)))
INSERT [dbo].[OrderDetail] ([order_id], [isbn], [quantity], [price]) VALUES (3, N'8934974151647', 5, CAST(220000 AS Decimal(8, 0)))
GO
SET IDENTITY_INSERT [dbo].[Promotion] ON 

INSERT [dbo].[Promotion] ([promotion_id], [promotion_name], [start_date], [end_date], [condition], [discount_percent], [quantity]) VALUES (2, N'Giảm giá đầu năm', CAST(N'2025-01-01' AS Date), CAST(N'2025-04-10' AS Date), CAST(500000 AS Decimal(8, 0)), CAST(0.15 AS Decimal(3, 2)), 7)
SET IDENTITY_INSERT [dbo].[Promotion] OFF
GO
SET IDENTITY_INSERT [dbo].[Staff] ON 

INSERT [dbo].[Staff] ([staff_id], [email], [fullname], [phone], [hash_pwd], [gender], [is_active], [is_ban]) VALUES (1, N'testuser@gmail.com', N'Nguyen Van A', N'0123456789', N'$2a$11$RQc0SMJYST8zrzbXOA7EaO3CYu/yS7liaj2MrGpZVz2yFXbTIML1y', 1, 0, 0)
INSERT [dbo].[Staff] ([staff_id], [email], [fullname], [phone], [hash_pwd], [gender], [is_active], [is_ban]) VALUES (7, N'phucnaoto@gmail.com', N'Thien Phuc', N'0123456788', N'$2a$11$HXncUmWc.JYMpknFyyUv6OOxhd4FGkgSWgHTdej0BNj5SEr0eKBt2', 1, 1, 0)
SET IDENTITY_INSERT [dbo].[Staff] OFF
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Category_name]    Script Date: 31-Mar-25 2:50:00 PM ******/
ALTER TABLE [dbo].[Category] ADD  CONSTRAINT [IX_Category_name] UNIQUE NONCLUSTERED 
(
	[category_name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Customer_phone]    Script Date: 31-Mar-25 2:50:00 PM ******/
ALTER TABLE [dbo].[Customer] ADD  CONSTRAINT [IX_Customer_phone] UNIQUE NONCLUSTERED 
(
	[customer_phone] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Staff_email]    Script Date: 31-Mar-25 2:50:00 PM ******/
ALTER TABLE [dbo].[Staff] ADD  CONSTRAINT [IX_Staff_email] UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [IX_Staff_phone]    Script Date: 31-Mar-25 2:50:00 PM ******/
ALTER TABLE [dbo].[Staff] ADD  CONSTRAINT [IX_Staff_phone] UNIQUE NONCLUSTERED 
(
	[phone] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Book] ADD  CONSTRAINT [DF_Book_is_discontinued]  DEFAULT ((0)) FOR [is_discontinued]
GO
ALTER TABLE [dbo].[Customer] ADD  CONSTRAINT [DF_Customer_customer_gender]  DEFAULT ((0)) FOR [customer_gender]
GO
ALTER TABLE [dbo].[Staff] ADD  CONSTRAINT [DF_Staff_gender]  DEFAULT ((0)) FOR [gender]
GO
ALTER TABLE [dbo].[Staff] ADD  CONSTRAINT [DF_Staff_is_active]  DEFAULT ((0)) FOR [is_active]
GO
ALTER TABLE [dbo].[Staff] ADD  CONSTRAINT [DF_Staff_is_ban]  DEFAULT ((0)) FOR [is_ban]
GO
ALTER TABLE [dbo].[Book]  WITH CHECK ADD  CONSTRAINT [FK_Book_Category] FOREIGN KEY([category_id])
REFERENCES [dbo].[Category] ([category_id])
GO
ALTER TABLE [dbo].[Book] CHECK CONSTRAINT [FK_Book_Category]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Customer] FOREIGN KEY([customer_id])
REFERENCES [dbo].[Customer] ([customer_id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Customer]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Promotion] FOREIGN KEY([promotion_id])
REFERENCES [dbo].[Promotion] ([promotion_id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Promotion]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Staff] FOREIGN KEY([staff_id])
REFERENCES [dbo].[Staff] ([staff_id])
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [FK_Order_Staff]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Book] FOREIGN KEY([isbn])
REFERENCES [dbo].[Book] ([isbn])
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Book]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetail_Order] FOREIGN KEY([order_id])
REFERENCES [dbo].[Order] ([order_id])
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [FK_OrderDetail_Order]
GO
ALTER TABLE [dbo].[Book]  WITH CHECK ADD  CONSTRAINT [CK_Book_price] CHECK  (([price]>=(10000)))
GO
ALTER TABLE [dbo].[Book] CHECK CONSTRAINT [CK_Book_price]
GO
ALTER TABLE [dbo].[Book]  WITH CHECK ADD  CONSTRAINT [CK_Book_quantity] CHECK  (([quantity]>=(0)))
GO
ALTER TABLE [dbo].[Book] CHECK CONSTRAINT [CK_Book_quantity]
GO
ALTER TABLE [dbo].[Book]  WITH CHECK ADD  CONSTRAINT [CK_Book_year] CHECK  (([year_of_publication]>(1900)))
GO
ALTER TABLE [dbo].[Book] CHECK CONSTRAINT [CK_Book_year]
GO
ALTER TABLE [dbo].[Order]  WITH CHECK ADD  CONSTRAINT [CK_Order_price] CHECK  (([total_price]>(10000)))
GO
ALTER TABLE [dbo].[Order] CHECK CONSTRAINT [CK_Order_price]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [CK_OrderDetail_price] CHECK  (([price]>(10000)))
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [CK_OrderDetail_price]
GO
ALTER TABLE [dbo].[OrderDetail]  WITH CHECK ADD  CONSTRAINT [CK_OrderDetail_quantity] CHECK  (([quantity]>(0)))
GO
ALTER TABLE [dbo].[OrderDetail] CHECK CONSTRAINT [CK_OrderDetail_quantity]
GO
ALTER TABLE [dbo].[Promotion]  WITH CHECK ADD  CONSTRAINT [CK_Promotion_condition] CHECK  (([condition]>(50000)))
GO
ALTER TABLE [dbo].[Promotion] CHECK CONSTRAINT [CK_Promotion_condition]
GO
ALTER TABLE [dbo].[Promotion]  WITH CHECK ADD  CONSTRAINT [CK_Promotion_date] CHECK  (([start_date]<=[end_date]))
GO
ALTER TABLE [dbo].[Promotion] CHECK CONSTRAINT [CK_Promotion_date]
GO
ALTER TABLE [dbo].[Promotion]  WITH CHECK ADD  CONSTRAINT [CK_Promotion_discount_percent] CHECK  (([discount_percent]>=(0.0)))
GO
ALTER TABLE [dbo].[Promotion] CHECK CONSTRAINT [CK_Promotion_discount_percent]
GO
ALTER TABLE [dbo].[Promotion]  WITH CHECK ADD  CONSTRAINT [CK_Promotion_quantity] CHECK  (([quantity]>=(0)))
GO
ALTER TABLE [dbo].[Promotion] CHECK CONSTRAINT [CK_Promotion_quantity]
GO
USE [master]
GO
ALTER DATABASE [Book_Api_Management] SET  READ_WRITE 
GO
