USE [QDAS_DATA]
GO
/****** Object:  StoredProcedure [dbo].[AddUpdateRoleDetails]    Script Date: 12/26/2023 5:21:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER  PROCEDURE [dbo].[AddUpdateRoleDetails]
(
	@p_role_id INT OUT,
	@p_role_code VARCHAR(30),
	@p_role_name VARCHAR(100),
	@p_landing_page varchar(100),
	@p_type INT, -- 1 for Add Record AND 2 for Update Record
	@p_status INT OUT
)
AS
BEGIN
	IF(@p_type = 1)
	BEGIN
		IF EXISTS(SELECT role_name FROM roles WHERE role_name = @p_role_name)
			OR EXISTS(SELECT role_code FROM roles WHERE role_code = @p_role_code)
		BEGIN
			SET @p_status = 2
			-- Record already exists!!
		END
		ELSE
		BEGIN
			INSERT INTO roles(role_code, role_name,landing_page,allow_edit)
			VALUES(@p_role_code, @p_role_name,@p_landing_page,1)

			SELECT @p_role_id = SCOPE_IDENTITY()
			SET @p_status = 1
			-- Record Inserted Successfully!!
		END
	END
	ELSE IF(@p_type = 2)
	BEGIN
		IF EXISTS(SELECT role_id FROM roles WHERE role_id = @p_role_id)
		BEGIN
			IF EXISTS(SELECT role_name FROM roles WHERE role_name = @p_role_name AND role_id <> @p_role_id)
				OR EXISTS(SELECT role_code FROM roles WHERE role_code = @p_role_code AND role_id <> @p_role_id)
			BEGIN
				SET @p_status = 2
				-- Record already exists!!
			END 
			ELSE
			BEGIN
				UPDATE roles
				SET role_code = ISNULL(@p_role_code, role_code),
				role_name = ISNULL(@p_role_name, role_name),
				landing_page = ISNULL(@p_landing_page,landing_page)
				WHERE role_id = @p_role_id

				IF(@@ROWCOUNT > 0)
				BEGIN
					SET @p_status = 3
					-- Record Updated Successfully!!
				END 
			END
		END
		ELSE
		BEGIN
			SET @p_status = 4
			-- Record does not exist!!
		END 
	END
END

GO
/****** Object:  StoredProcedure [dbo].[AddUser]    Script Date: 12/26/2023 5:21:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[AddUser]
( 
    @p_username VARCHAR(100),
    @p_full_name VARCHAR(1000),
	@p_email varchar(1000),
	@p_status INT OUT
)
AS
BEGIN
	IF NOT EXISTS (SELECT username FROM users WHERE username = @p_username )
		BEGIN
			INSERT INTO users(username,full_name,email,role_id) VALUES(@p_username,@p_full_name,@p_email,1)
			SELECT @p_status = 1 -- USER ADDED SUCCESSFULLY
		END
	ELSE
		BEGIN
			SELECT @p_status = -1 -- USER IS ALREADY EXIST
		END
END



GO
/****** Object:  StoredProcedure [dbo].[DeleteRole]    Script Date: 12/26/2023 5:21:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[DeleteRole]
(
	@p_role_id INT,
	@p_status INT OUT
)
AS
BEGIN
	IF EXISTS(SELECT role_id FROM users WHERE role_id = @p_role_id)
	OR EXISTS(SELECT role_id FROM role_permissions WHERE role_id = @p_role_id )
	BEGIN
			SELECT @p_status = -1
			-- role_id is in use
	END
	ELSE IF EXISTS(SELECT role_id FROM roles WHERE role_id = @p_role_id)
	BEGIN
			DELETE FROM roles
			WHERE  role_id = @p_role_id
			SET @p_status = 1
			-- Record Deleted Successfully!!
	END
	ELSE 
	BEGIN
			SET @p_status = 2
			-- Record does not exist!!
	END
END
GO
/****** Object:  StoredProcedure [dbo].[GetRoleDetailsById]    Script Date: 12/26/2023 5:21:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetRoleDetailsById]
(
	@p_role_id INT
)
AS
BEGIN
	SELECT role_id, role_code, role_name
	FROM roles
	WHERE role_id = @p_role_id
END
GO
/****** Object:  StoredProcedure [dbo].[GetRoleList]    Script Date: 12/26/2023 5:21:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE   PROCEDURE [dbo].[GetRoleList]
AS
BEGIN
	SELECT role_id, role_code, role_name
	FROM roles
	--where role_code != 'DMYROL' and allow_edit != 0 
	ORDER BY role_id
	
END
GO
/****** Object:  StoredProcedure [dbo].[GetRolePermissionList]    Script Date: 12/26/2023 5:21:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[GetRolePermissionList]
(
	@p_role_id INT
)
AS
BEGIN
	SELECT RP.id as rp_id, RL.role_id, RL.role_code, RL.role_name, PR.id as pr_id, PR.page_code, PR.page_name, PR.code, PR.permission_name, PR.description,
	(CASE WHEN (RL.role_code = 'ADMIN' OR RP.role_id > 0) THEN CONVERT(BIT, 1) ELSE CONVERT(BIT, 0) END) AS pr_granted,
	PR.page_code + '.' + PR.code AS permission_code
	FROM roles RL
	CROSS JOIN permissions PR
	LEFT OUTER JOIN role_permissions RP
	ON PR.id = RP.permission_id
	AND RL.role_id = RP.role_id
	WHERE RL.role_id = @p_role_id
	ORDER BY RP.id 
END

GO
/****** Object:  StoredProcedure [dbo].[GetUserDetailsByUserName]    Script Date: 12/26/2023 5:21:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE  OR ALTER PROCEDURE [dbo].[GetUserDetailsByUserName]
(
	@p_username varchar(100) 
)
AS
BEGIN
	SELECT US.id, US.username,US.full_name, US.email, 
	US.role_id, RL.role_code, RL.role_name,US.plant_location, US.area, RL.landing_page
	FROM users US
	INNER JOIN Roles RL
	ON US.role_id = RL.role_id
	WHERE US.username = @p_username
END
GO
/****** Object:  StoredProcedure [dbo].[SaveRolePermissionInfo]    Script Date: 12/26/2023 5:21:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE OR ALTER PROCEDURE [dbo].[SaveRolePermissionInfo]
(
	@p_rp_id INT,
	@p_role_id INT,
	@p_pr_id INT,
	@p_pr_granted BIT,
	@p_type INT, -- 1 for Add Record AND 2 for Update Record
	@p_status INT OUT
)
AS
BEGIN
	IF(@p_type = 1 OR @p_type = 2)
	BEGIN
		IF(@p_pr_granted = 1)
		BEGIN
			IF NOT EXISTS(SELECT rp.id FROM role_permissions rp WHERE rp.role_id = @p_role_id AND rp.permission_id = @p_pr_id)
			BEGIN
				INSERT INTO role_permissions(role_id, permission_id)
				VALUES(@p_role_id, @p_pr_id)

				IF(@@ROWCOUNT > 0)
				BEGIN
					SELECT @p_rp_id = SCOPE_IDENTITY()
					SELECT @p_status = 1
					-- Record Inserted Successfully!!
				END
			END
			ELSE
			BEGIN
				UPDATE role_permissions
				SET role_id = ISNULL(@p_role_id, role_id),
				permission_id = ISNULL(@p_pr_id, permission_id)
				WHERE id = @p_rp_id

				IF(@@ROWCOUNT > 0)
				BEGIN
					SELECT @p_status = 3
					-- Record Updated Successfully!!
				END
			END
		END
		ELSE
		BEGIN
			-- EXEC [role_permissions] @p_rp_id, @p_status OUT
			DELETE FROM role_permissions
			WHERE role_id = @p_role_id
			AND permission_id = @p_pr_id

			IF(@@ROWCOUNT > 0)
			BEGIN
				SELECT @p_status = 1
				-- Record Deleted Successfully!!
			END
		END
	END
END

GO
/****** Object:  StoredProcedure [dbo].[UpdateUser]    Script Date: 12/26/2023 5:21:56 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER PROCEDURE [dbo].[UpdateUser]
 (
    @p_id INT,
    @p_role_id VARCHAR(1000),
	@p_plant_location VARCHAR(100),
	@p_area VARCHAR(100),
	@p_status INT OUT
)
AS
BEGIN
	IF EXISTS (select id  from users where id = @p_id )
		BEGIN
			UPDATE users SET role_id =  @p_role_id, plant_location = @p_plant_location, area = @p_area    WHERE id = @p_id 
			SET @p_status = 1 -- user updated successfully
		END
	ELSE
		BEGIN
			SET @p_status = -1 -- user is not exist
		END
	
END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[permissions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[code] [varchar](100) NOT NULL,
	[permission_name] [varchar](100) NOT NULL,
	[page_code] [varchar](100) NOT NULL,
	[page_name] [varchar](100) NOT NULL,
	[description] [varchar](1000) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[role_permissions]    Script Date: 12/26/2023 5:38:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[role_permissions](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[role_id] [int] NOT NULL,
	[permission_id] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[roles]    Script Date: 12/26/2023 5:38:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[roles](
	[role_id] [int] IDENTITY(1,1) NOT NULL,
	[role_code] [varchar](100) NOT NULL,
	[role_name] [varchar](1000) NOT NULL,
	[landing_page] [varchar](100) NULL,
	[allow_edit] [bit] NULL,
PRIMARY KEY CLUSTERED 
(
	[role_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 12/26/2023 5:38:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[username] [varchar](100) NOT NULL,
	[full_name] [varchar](1000) NOT NULL,
	[email] [varchar](1000) NOT NULL,
	[role_id] [int] NOT NULL,
	[plant_location] [varchar](100) NULL,
	[area] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET IDENTITY_INSERT [dbo].[permissions] ON 
GO
INSERT [dbo].[permissions] ([id], [code], [permission_name], [page_code], [page_name], [description]) VALUES (1, N'VIEW_CVBU', N'View CVBU', N'CVBU', N'CVBU', N'CVBU Page')
GO
INSERT [dbo].[permissions] ([id], [code], [permission_name], [page_code], [page_name], [description]) VALUES (2, N'VIEW_PLANT_STATUS', N'View Plant Status', N'PLANT_STATUS', N'Plant Status', N'Plant Status Page')
GO
INSERT [dbo].[permissions] ([id], [code], [permission_name], [page_code], [page_name], [description]) VALUES (3, N'VIEW_AREA_EXPLORER', N'View Area Explorer', N'AREA_EXPLORER', N'Area Explorer', N'Area Explorer Page')
GO
INSERT [dbo].[permissions] ([id], [code], [permission_name], [page_code], [page_name], [description]) VALUES (4, N'VIEW_2x2_MATRIX', N'View 2x2 Matrix', N'2X2_MATRIX', N'2x2 Matrix', N'2x2 Matrix Page')
GO
INSERT [dbo].[permissions] ([id], [code], [permission_name], [page_code], [page_name], [description]) VALUES (5, N'VIEW_TARGET_INSPECTION', N'View Target Inspection', N'TARGET_INSPECTION', N'Target Inspection', N'Target Inspection Page')
GO
INSERT [dbo].[permissions] ([id], [code], [permission_name], [page_code], [page_name], [description]) VALUES (6, N'VIEW_CHAR_EXPLORER', N'View Char Explorer', N'CHAR_EXPLORER', N'Characteristic Explorer', N'Char Explorer Page')
GO
INSERT [dbo].[permissions] ([id], [code], [permission_name], [page_code], [page_name], [description]) VALUES (7, N'VIEW_MODEL_EXPLORER', N'View Model Explorer', N'MODEL_EXPLORER', N'Model Explorer', N'Model Explorer Page')
GO
INSERT [dbo].[permissions] ([id], [code], [permission_name], [page_code], [page_name], [description]) VALUES (8, N'VIEW_CONFIG_USER', N'View Confing User', N'CONFIG_USER', N'Confing User', N'Confing User Page')
GO
INSERT [dbo].[permissions] ([id], [code], [permission_name], [page_code], [page_name], [description]) VALUES (9, N'VIEW_CONFIG_PERMISSION', N'View Config Permission', N'CONFIG_PERMISSION', N'Config Permission', N'Config Permission Page')
GO
INSERT [dbo].[permissions] ([id], [code], [permission_name], [page_code], [page_name], [description]) VALUES (10, N'VIEW_CONFIG_ROLE', N'View Config Role', N'CONFIG_ROLE', N'Config Role', N'Config Role Page')
GO
SET IDENTITY_INSERT [dbo].[permissions] OFF
GO
SET IDENTITY_INSERT [dbo].[role_permissions] ON 
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (1, 2, 1)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (2, 2, 2)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (3, 2, 3)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (4, 2, 4)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (5, 2, 5)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (6, 2, 6)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (7, 2, 7)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (8, 2, 8)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (9, 2, 9)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (10, 2, 10)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (18, 4, 3)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (19, 4, 6)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (20, 4, 7)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (77, 12, 4)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (78, 12, 3)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (79, 12, 6)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (80, 12, 5)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (81, 12, 7)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (82, 12, 10)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (83, 12, 9)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (94, 14, 1)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (95, 14, 2)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (96, 14, 3)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (97, 14, 4)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (98, 14, 5)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (99, 14, 6)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (104, 14, 7)
GO
INSERT [dbo].[role_permissions] ([id], [role_id], [permission_id]) VALUES (106, 12, 2)
GO
SET IDENTITY_INSERT [dbo].[role_permissions] OFF
GO
SET IDENTITY_INSERT [dbo].[roles] ON 
GO
INSERT [dbo].[roles] ([role_id], [role_code], [role_name], [landing_page], [allow_edit]) VALUES (1, N'DMYROL', N'DummyRole', NULL, 0)
GO
INSERT [dbo].[roles] ([role_id], [role_code], [role_name], [landing_page], [allow_edit]) VALUES (2, N'ADMIN', N'Admin', N'CVBU', 1)
GO
INSERT [dbo].[roles] ([role_id], [role_code], [role_name], [landing_page], [allow_edit]) VALUES (4, N'SUPRV', N'Supervisor', N'AreaExplorer', 1)
GO
INSERT [dbo].[roles] ([role_id], [role_code], [role_name], [landing_page], [allow_edit]) VALUES (7, N'testing', N'testing', NULL, 1)
GO
INSERT [dbo].[roles] ([role_id], [role_code], [role_name], [landing_page], [allow_edit]) VALUES (12, N'PLTMNGT', N'Plant Manager', N'plantStatus', 1)
GO
INSERT [dbo].[roles] ([role_id], [role_code], [role_name], [landing_page], [allow_edit]) VALUES (14, N'MNGT', N'Management', N'CVBU', 1)
GO
SET IDENTITY_INSERT [dbo].[roles] OFF
GO
SET IDENTITY_INSERT [dbo].[users] ON 
GO
INSERT [dbo].[users] ([id], [username], [full_name], [email], [role_id], [plant_location], [area]) VALUES (23, N'super', N'supervisor', N'cvhdjh', 1, N'', N'')
GO
INSERT [dbo].[users] ([id], [username], [full_name], [email], [role_id], [plant_location], [area]) VALUES (24, N'manager', N'manager', N'manager', 1, N'PNT', N'GRB')
GO
INSERT [dbo].[users] ([id], [username], [full_name], [email], [role_id], [plant_location], [area]) VALUES (26, N'newton', N'Isaac Newton', N'newton@ldap.forumsys.com', 2, N'PNE', N'ENG')
GO
INSERT [dbo].[users] ([id], [username], [full_name], [email], [role_id], [plant_location], [area]) VALUES (29, N'einstein', N'Albert Einstein', N'einstein@ldap.forumsys.com', 1, NULL, NULL)
GO
SET IDENTITY_INSERT [dbo].[users] OFF
GO
ALTER TABLE [dbo].[permissions] ADD  DEFAULT (NULL) FOR [description]
GO
ALTER TABLE [dbo].[role_permissions]  WITH CHECK ADD  CONSTRAINT [FK_role_permissions_roles] FOREIGN KEY([role_id])
REFERENCES [dbo].[roles] ([role_id])
GO
ALTER TABLE [dbo].[role_permissions] CHECK CONSTRAINT [FK_role_permissions_roles]
GO
ALTER TABLE [dbo].[role_permissions]  WITH CHECK ADD  CONSTRAINT [FK_RolePermission_Permission] FOREIGN KEY([permission_id])
REFERENCES [dbo].[permissions] ([id])
GO
ALTER TABLE [dbo].[role_permissions] CHECK CONSTRAINT [FK_RolePermission_Permission]
GO
ALTER TABLE [dbo].[users]  WITH CHECK ADD  CONSTRAINT [FK_users_roles] FOREIGN KEY([role_id])
REFERENCES [dbo].[roles] ([role_id])
GO
ALTER TABLE [dbo].[users] CHECK CONSTRAINT [FK_users_roles]
GO

