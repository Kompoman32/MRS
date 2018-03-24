
SET IDENTITY_INSERT [dbo].[UserSet] ON
INSERT INTO [dbo].[UserSet] ([Id], [Login], [Password], [FullName], [AdminPrivileges]) VALUES (1, N'admin', N'admin', N'Липин Слава', 1)
INSERT INTO [dbo].[UserSet] ([Id], [Login], [Password], [FullName], [AdminPrivileges]) VALUES (2, N'master32', N'123qwe', N'Александр Трегубов', 1)
INSERT INTO [dbo].[UserSet] ([Id], [Login], [Password], [FullName], [AdminPrivileges]) VALUES (3, N'miha2158', N'8512ahim', N'Михаил Иванов', 0)
INSERT INTO [dbo].[UserSet] ([Id], [Login], [Password], [FullName], [AdminPrivileges]) VALUES (4, N'jeka34', N'IMINLOVE', N'Евгений Камушкин', 0)
INSERT INTO [dbo].[UserSet] ([Id], [Login], [Password], [FullName], [AdminPrivileges]) VALUES (5, N'trofim42', N'trofa42', N'Трофимов Илья Сергеевич', 0)
SET IDENTITY_INSERT [dbo].[UserSet] OFF


SET IDENTITY_INSERT [dbo].[TypeSet] ON
INSERT INTO [dbo].[TypeSet] ([Id], [Name], [Unit]) VALUES (1, N'Электрический', N'кВт/ч')
INSERT INTO [dbo].[TypeSet] ([Id], [Name], [Unit]) VALUES (2, N'Водный', N'м^3')
INSERT INTO [dbo].[TypeSet] ([Id], [Name], [Unit]) VALUES (3, N'Газовый', N'м^3')
INSERT INTO [dbo].[TypeSet] ([Id], [Name], [Unit]) VALUES (4, N'Тепловой', N'Дж')
SET IDENTITY_INSERT [dbo].[TypeSet] OFF


SET IDENTITY_INSERT [dbo].[TariffSet] ON
INSERT INTO [dbo].[TariffSet] ([Id], [Name]) VALUES (1, N'Одинарный')
INSERT INTO [dbo].[TariffSet] ([Id], [Name]) VALUES (2, N'Двойной')
INSERT INTO [dbo].[TariffSet] ([Id], [Name]) VALUES (3, N'Тройной')
SET IDENTITY_INSERT [dbo].[TariffSet] OFF

SET IDENTITY_INSERT [dbo].[TimeSpanSet] ON
INSERT INTO [dbo].[TimeSpanSet] ([Id], [Name], [TimeStart], [TimeEnd], [Tariff_Id]) VALUES (1, N'Единый', N'00:00:00', N'23:59:00', 1)
INSERT INTO [dbo].[TimeSpanSet] ([Id], [Name], [TimeStart], [TimeEnd], [Tariff_Id]) VALUES (2, N'Дневной(2-й)', N'07:00:00', N'22:59:00', 2)
INSERT INTO [dbo].[TimeSpanSet] ([Id], [Name], [TimeStart], [TimeEnd], [Tariff_Id]) VALUES (3, N'Ночной(2-й)', N'23:00:00', N'06:59:00', 2)
INSERT INTO [dbo].[TimeSpanSet] ([Id], [Name], [TimeStart], [TimeEnd], [Tariff_Id]) VALUES (4, N'Полупиковый_День(3-й)', N'10:00:00', N'16:59:00', 3)
INSERT INTO [dbo].[TimeSpanSet] ([Id], [Name], [TimeStart], [TimeEnd], [Tariff_Id]) VALUES (5, N'Полупиковый_Вечер(3-й)', N'21:00:00', N'22:59:00', 3)
INSERT INTO [dbo].[TimeSpanSet] ([Id], [Name], [TimeStart], [TimeEnd], [Tariff_Id]) VALUES (6, N'Пиковый_Утро(3-й)', N'07:00:00', N'09:59:00', 3)
INSERT INTO [dbo].[TimeSpanSet] ([Id], [Name], [TimeStart], [TimeEnd], [Tariff_Id]) VALUES (7, N'Пиковый_Вечер', N'17:00:00', N'20:59:00', 3)
INSERT INTO [dbo].[TimeSpanSet] ([Id], [Name], [TimeStart], [TimeEnd], [Tariff_Id]) VALUES (8, N'Ночной(3-й)', N'23:00:00', N'06:59:00', 3)
SET IDENTITY_INSERT [dbo].[TimeSpanSet] OFF

SET IDENTITY_INSERT [dbo].[ParametrSet] ON
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (1, N'Разрядность', N'1')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (2, N'Разрядность', N'2')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (3, N'Давление', N'386 Па')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (4, N'Давление', N'100 Па')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (8, N'Давление', N'128 Па')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (9, N'Точность', N'2,0-2,5')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (11, N'Количство фаз', N'1')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (12, N'Количство фаз', N'2')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (13, N'Количество ', N'3')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (14, N'Номинальный ток', N'5 А')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (15, N'Номинальный ток', N'10 А')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (16, N'Номинальный', N'20 А')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (17, N'Номинальный ток', N'40 А')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (18, N'Номинальный ток', N'60 А')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (19, N'Напряжение ', N'220 В')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (20, N'Напряжение', N'127 В')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (21, N'Цвет', N'Серый')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (22, N'Цвет', N'Зелёный')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (23, N'Цвет', N'Красный')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (24, N'Цвет', N'Белый')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (25, N'Цвет', N'чёрный')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (26, N'Размер', N'3 дюйма')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (27, N'Размер', N'30х10 см')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (28, N'Расход воды', N'0,6 м^3')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (29, N'Расход воды', N'1 м^3')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (30, N'Расход воды', N'1,5 м^3')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (31, N'Расход воды', N'2,5 м^3')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (32, N'Диаметр трубы', N'10 мм')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (33, N'Диаметр трубы', N'12 мм')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (34, N'Пропуская способность', N'25 м^3')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (35, N'Максимальная температура', N'120 градусов')
INSERT INTO [dbo].[ParametrSet] ([Id], [Name], [Measure]) VALUES (36, N'Минимальная температура', N'60 градусов')
SET IDENTITY_INSERT [dbo].[ParametrSet] OFF

