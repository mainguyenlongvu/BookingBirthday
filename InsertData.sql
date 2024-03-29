use BookingBirthdayDb

INSERT INTO [User] (Name, Gender, DateOfBirth, Username, Password, Email, Phone, Address, Image_url, Role, Status) VALUES
('Admin', 'Nam', '2000-01-01', 'admin12345', '25d55ad283aa400af464c76d713c07ad', 'admin@gmail.com', '0123456789', 'abc', '/imgProfile/avatar.png', 'Admin', 'Active'),
('Guest', N'Nữ', '2000-01-01', 'guest12345', '25d55ad283aa400af464c76d713c07ad', 'example123@gmail.com', '9876543210', 'abc', '/imgProfile/avatar.png', 'Guest', 'Active'),
('Host', 'Nam', '2000-01-01', 'host12345', '25d55ad283aa400af464c76d713c07ad', 'demo5678@gmail.com', '1122334455', 'abc', '/imgProfile/avatar.png', 'Host', 'Active');



INSERT INTO [Area] (Name) VALUES
(N'Quận 1'),
(N'Quận 2'),
(N'Quận 3'),
(N'Quận Thủ Đức'),
(N'Quận Bình Thạnh'),
(N'Quận Gò Vấp'),
(N'Quận Phú Nhuận')


INSERT INTO [Location] (Name, Address, AreaId, Status) VALUES
(N'Jollibee', N'123 Đường Nguyễn Huệ, Phường Bến Thành, Quận 1, Thành phố Hồ Chí Minh', 1, 'Active'),
(N'KFC', N'456 Đường Thảo Điền, Phường Thảo Điền, Quận 2, Thành phố Hồ Chí Minh', 2, 'Active'),
(N'KFC', N'789 Đường Nguyễn Văn Linh, Phường Phạm Ngũ Lão, Quận 3, Thành phố Hồ Chí Minh', 3, 'Active'),
(N'KFC', N'102 Đặng Văn Bi, Bình Thọ, Q. Thủ Đức, TP. HCM', 4, 'Active'),
(N'KFC', N'123 Đường Nguyễn Huệ, Phường Bến Thành, Quận 1, Thành phố Hồ Chí Minh', 1, 'Active'),
(N'Jollibee', N'456 Đường Thảo Điền, Phường Thảo Điền, Quận 2, Thành phố Hồ Chí Minh', 1, 'Active'),
(N'Jollibee', N'789 Đường Nguyễn Văn Linh, Phường Phạm Ngũ Lão, Quận 3, Thành phố Hồ Chí Minh', 3, 'Active'),
(N'KFC', N'102 Đặng Văn Bi, Bình Thọ, Q. Thủ Đức, TP. HCM', 1, 'Active')



INSERT INTO Theme ([Name] ,[UserId] ,[Status]) Values
(N'Pikachu',3,N'Active'),
(N'Doraemon',3,N'Active'),
(N'Hello Kitty',3,N'Active'),
(N'Minion',3,N'Active'),
(N'Super Car',3,N'Active'),
(N'Khủng long',3,N'Active'),
(N'Phù thủy',3,N'Active')


