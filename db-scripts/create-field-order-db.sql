-- Create Database
CREATE DATABASE FieldOrdersDB;
GO

USE FieldOrdersDB;
GO

-- Create Schemas
CREATE SCHEMA [User];
GO

CREATE SCHEMA [Company];
GO

CREATE SCHEMA [Project];
GO

CREATE SCHEMA [Lookup];
GO

CREATE SCHEMA [Order];
GO

-- User table
CREATE TABLE [User].[User](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Password] [varchar](100) NOT NULL,
	[FirstName] [varchar](100) NOT NULL,
	[LastName] [varchar](100) NOT NULL,
	[PasswordExpiry] [date] NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [User].[User] ADD  CONSTRAINT [DF_User_isActive]  DEFAULT ((0)) FOR [IsActive]
GO

-- Lookups Tables
-----------------
-- Order Status
CREATE TABLE [Lookup].[OrderStatus](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [varchar](30) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_OrderStatus] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Lookup].[OrderStatus] ADD  CONSTRAINT [DF_OrderStatus_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO

-- Shipping Method
CREATE TABLE [Lookup].[ShippingMethod](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [varchar](50) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL
 CONSTRAINT [PK_ShippingMethod] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Lookup].[ShippingMethod] ADD  CONSTRAINT [DF_ShippingMethod_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO

-- Company table
CREATE TABLE [Company].[Company](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Number] [varchar](25) NOT NULL,
	[Name] [varchar](75) NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL
 CONSTRAINT [PK_Company] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Company].[Company] ADD  CONSTRAINT [DF_Company_ActiveYN]  DEFAULT ((0)) FOR [IsActive]
GO

-- Projects table
CREATE TABLE [Project].[Project](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[CompanyID] [int] NOT NULL,
	[Number] [varchar](15) NOT NULL,
	[Name] [varchar](150) NOT NULL,
	[Description] [varchar](150) NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL
 CONSTRAINT [PK_Project] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Project].[Project] ADD  CONSTRAINT [DF_Project_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO

ALTER TABLE [Project].[Project]  WITH CHECK ADD  CONSTRAINT [FK_Project_Company] FOREIGN KEY([CompanyID])
REFERENCES [Company].[Company] ([ID])
GO

ALTER TABLE [Project].[Project] CHECK CONSTRAINT [FK_Project_Company]
GO

-- Project WBS table
CREATE TABLE [Project].[ProjectWBS](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[TaskCode] [varchar](25) NOT NULL,
	[TaskDescription] [varchar](50) NOT NULL,
	[Budget] [money] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_ProjectWBS] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Project].[ProjectWBS] ADD  CONSTRAINT [DF_ProjectWBS_IsActive]  DEFAULT ((0)) FOR [IsActive]
GO

ALTER TABLE [Project].[ProjectWBS]  WITH CHECK ADD  CONSTRAINT [FK_ProjectWBS_Project] FOREIGN KEY([ProjectID])
REFERENCES [Project].[Project] ([ID])
GO

ALTER TABLE [Project].[ProjectWBS] CHECK CONSTRAINT [FK_ProjectWBS_Project]
GO

CREATE TABLE [Order].[Order](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[ProjectID] [int] NOT NULL,
	[Number] [varchar](50) NOT NULL,
	[Name] [varchar](255) NOT NULL,
	[StatusID] [int] NOT NULL,
	[OrderType] [varchar](30) NOT NULL,
	[InitiatedDate] [datetime] NULL,
	[RasDate] [datetime] NULL,
	[GoodsReceived] [datetime] NULL,
	[DeliveryPoint] [varchar](500) NULL,
	[OriginatorID] [int] NULL,
	[ReadyForPurchase] [bit] NOT NULL,
	[IssuedToVendor] [bit] NOT NULL,
	[ShippingMethodID] [int] NULL,
	[CloseOutDate] [date] NULL,
	[GST] [decimal](5, 2) NULL,
	[PST] [decimal](5, 2) NULL,
	[HST] [decimal](5, 2) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL

 CONSTRAINT [PK_Order] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [UQ_Order_OrderId_OrderType] UNIQUE NONCLUSTERED 
(
	[ID] ASC,
	[OrderType] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Order].[Order] ADD  CONSTRAINT [DF_Order_ReadyForPurchase]  DEFAULT ((0)) FOR [ReadyForPurchase]
GO

ALTER TABLE [Order].[Order] ADD  CONSTRAINT [DF_Order_IssuedToVendor]  DEFAULT ((0)) FOR [IssuedToVendor]
GO

ALTER TABLE [Order].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_OrderStatus] FOREIGN KEY([StatusID])
REFERENCES [Lookup].[OrderStatus] ([ID])
GO

ALTER TABLE [Order].[Order] CHECK CONSTRAINT [FK_Order_OrderStatus]
GO

ALTER TABLE [Order].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_Project] FOREIGN KEY([ProjectID])
REFERENCES [Project].[Project] ([ID])
GO

ALTER TABLE [Order].[Order] CHECK CONSTRAINT [FK_Order_Project]
GO

ALTER TABLE [Order].[Order]  WITH CHECK ADD  CONSTRAINT [FK_Order_ShippingMethod] FOREIGN KEY([ShippingMethodID])
REFERENCES [Lookup].[ShippingMethod] ([ID])
GO

ALTER TABLE [Order].[Order] CHECK CONSTRAINT [FK_Order_ShippingMethod]
GO

-- Order Item table
CREATE TABLE [Order].[OrderItem](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[ItemNumber] [int] NOT NULL,
	[Quantity] [decimal](8, 2) NULL,
	[UOM] [varchar](50) NOT NULL,
	[Description] [varchar](max) NOT NULL,
	[WbsID] [int] NULL,
	[UnitPrice] [money] NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_OrderItem] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [Order].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_Order] FOREIGN KEY([OrderID])
REFERENCES [Order].[Order] ([ID])
GO

ALTER TABLE [Order].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_Order]
GO

ALTER TABLE [Order].[OrderItem]  WITH CHECK ADD  CONSTRAINT [FK_OrderItem_ProjectWBS] FOREIGN KEY([WbsID])
REFERENCES [Project].[ProjectWBS] ([ID])
GO

ALTER TABLE [Order].[OrderItem] CHECK CONSTRAINT [FK_OrderItem_ProjectWBS]
GO

-- Order Comment table
CREATE TABLE [Order].[OrderComment](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[Comment] [varchar](500) NOT NULL,
	[CommentDate] [date] NOT NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL
 CONSTRAINT [PK_OrderComment] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Order].[OrderComment]  WITH CHECK ADD  CONSTRAINT [FK_OrderComment_Order] FOREIGN KEY([OrderID])
REFERENCES [Order].[Order] ([ID])
GO

ALTER TABLE [Order].[OrderComment] CHECK CONSTRAINT [FK_OrderComment_Order]
GO

-- Field Vendor table
CREATE TABLE [Company].[FieldVendor](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[CompanyName] [varchar](75) NULL,
	[CompanyID] [int] NULL,
	[ContactFirstName] [varchar](20) NULL,
	[ContactLastName] [varchar](15) NULL,
	[ContactPhone] [varchar](50) NULL,
	[ContactEmail] [varchar](100) NULL,
	[ContactCell] [varchar](50) NULL,
	[ContactFax] [varchar](50) NULL,
	[LocationAddress] [varchar](200) NULL,
	[LocationCity] [nvarchar](50) NULL,
	[LocationState] [nvarchar](50) NULL,
	[LocationCountry] [nvarchar](50) NULL,
	[LocationPostalCode] [varchar](15) NULL,
	[LocationPhone] [varchar](50) NULL,
	[LocationFax] [varchar](50) NULL,
	[LocationEmail] [varchar](100) NULL,
	[CreatedBy] [int] NULL,
	[CreatedDate] [datetime] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_FieldVendor] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Company].[FieldVendor]  WITH CHECK ADD  CONSTRAINT [FK_FieldVendor_Company] FOREIGN KEY([CompanyID])
REFERENCES [Company].[Company] ([ID])
GO

ALTER TABLE [Company].[FieldVendor] CHECK CONSTRAINT [FK_FieldVendor_Company]
GO

ALTER TABLE [Company].[FieldVendor]  WITH CHECK ADD  CONSTRAINT [FK_FieldVendor_Order] FOREIGN KEY([OrderID])
REFERENCES [Order].[Order] ([ID])
GO

ALTER TABLE [Company].[FieldVendor] CHECK CONSTRAINT [FK_FieldVendor_Order]
GO

ALTER TABLE [Company].[FieldVendor]  WITH CHECK ADD  CONSTRAINT [FieldVendorContactAndLocationMustHaveCompanyName] CHECK  (([ContactFirstName] IS NULL AND [ContactLastName] IS NULL AND [ContactPhone] IS NULL AND [ContactEmail] IS NULL AND [ContactCell] IS NULL AND [ContactFax] IS NULL AND [LocationAddress] IS NULL AND [LocationCity] IS NULL AND [LocationState] IS NULL AND [LocationCountry] IS NULL AND [LocationPostalCode] IS NULL AND [LocationPhone] IS NULL AND [LocationFax] IS NULL AND [LocationEmail] IS NULL OR [CompanyName] IS NOT NULL))
GO

ALTER TABLE [Company].[FieldVendor] CHECK CONSTRAINT [FieldVendorContactAndLocationMustHaveCompanyName]
GO

ALTER TABLE [Company].[FieldVendor]  WITH CHECK ADD  CONSTRAINT [FieldVendorContactMustHaveLocation] CHECK  (([ContactFirstName] IS NULL AND [ContactLastName] IS NULL AND [ContactPhone] IS NULL AND [ContactEmail] IS NULL AND [ContactCell] IS NULL AND [ContactFax] IS NULL OR [LocationAddress] IS NOT NULL AND [LocationCity] IS NOT NULL))
GO

ALTER TABLE [Company].[FieldVendor] CHECK CONSTRAINT [FieldVendorContactMustHaveLocation]
GO

 -- Recent Orders table
CREATE TABLE [Order].[RecentOrder](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NOT NULL,
	[OrderID] [int] NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [int] NULL,
	[ModifiedBy] [int] NULL,
	[ModifiedDate] [datetime] NULL,
 CONSTRAINT [PK_RecentOrder] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [Order].[RecentOrder]  WITH CHECK ADD  CONSTRAINT [FK_RecentOrder_Order] FOREIGN KEY([OrderID])
REFERENCES [Order].[Order] ([ID])
GO

ALTER TABLE [Order].[RecentOrder] CHECK CONSTRAINT [FK_RecentOrder_Order]
GO

ALTER TABLE [Order].[RecentOrder]  WITH CHECK ADD  CONSTRAINT [FK_RecentOrder_User] FOREIGN KEY([UserID])
REFERENCES [User].[User] ([ID])
GO

ALTER TABLE [Order].[RecentOrder] CHECK CONSTRAINT [FK_RecentOrder_User]
GO




