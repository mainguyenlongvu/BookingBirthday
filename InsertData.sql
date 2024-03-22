use BookingBirthdayDb

INSERT INTO [User] (Name, Gender, DateOfBirth, Username, Password, Email, Phone, Address, Image_url, Role, Status) VALUES
('Admin', 'Nam', '2000-01-01', 'admin12345', '25d55ad283aa400af464c76d713c07ad', 'admin@gmail.com', '0123456789', 'abc', '/imgProfile/avatar.png', 'Admin', 'Active'),
('Guest', N'Nữ', '2000-01-01', 'guest12345', '25d55ad283aa400af464c76d713c07ad', 'example123@gmail.com', '9876543210', 'abc', '/imgProfile/avatar.png', 'Guest', 'Active'),
('Host', 'Nam', '2000-01-01', 'host12345', '25d55ad283aa400af464c76d713c07ad', 'demo5678@gmail.com', '1122334455', 'abc', '/imgProfile/avatar.png', 'Host', 'Active');

insert into [Categories] (name) values
('500k - 1tr'),
('1tr - 2tr'),
('2tr - 3tr'),
('3tr - 4tr'),
('4tr - 5tr'),
('> 5tr')