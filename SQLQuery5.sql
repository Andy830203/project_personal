INSERT INTO brand (brand_name) VALUES 
('Asics'),
('Babolat'),
('Dunlop'),
('Head'),
('Lacoste'),
('Luxilon'),
('Nike'),
('Slazenger'),
('Solinco'),
('Tecnifibre'),
('Wilson'),
('Yonex');

INSERT INTO category (category_name) VALUES 
('Racquets'),
('Tennis Shoes'),
('Apparel'),
('Tennis Balls'),
('Tennis Bags'),
('Grip and Accessories'),
('Strings');

INSERT INTO user_level VALUES
(1, 'shopkeeper'),
(2, 'manager'),
(3, 'employee'),
(4, 'costomer');

INSERT INTO user_status VALUES
(1, 'ACTIVE'),
(2, 'BANNED'),
(3, 'DELETED');



INSERT INTO size VALUES
(1, 'XS'),
(2, 'S'),
(3, 'M'),
(4, 'L'),
(5, 'XL'),
(6, 'XXL'),
(7, '7'),
(8, '8'),
(9, '9'),
(10,'10'),
(11, '11'),
(12, '7.5'),
(13, '8.5'),
(14, '9.5'),
(15, '10.5');

INSERT INTO member (username, password, userlevel, userstatus, real_name, phone, email, address, birthday) VALUES
('rf810376', 'd122744587', 1, 1, 'Roger Federer', '0988080808', 'kingRoger@gmail.com', 'Church Rd, London SW19 5AG', '1981-08-08');

INSERT INTO product (product_name, brand, category, price, on_shelf, description) VALUES 
('Wilson US Open Regular Duty Tennis Ball Can', 11, 4, 140, 1, 'The US Open Regular Duty Tennis Ball is USTA and ITF approved for competitive play and is designed with durability and consistency in mind.');

INSERT INTO product (product_name, brand, category, price, on_shelf, description) VALUES 
('Babolat Pure Aero Rafa Origin', 2, 1, 7000, 1, 'Unlike the standard Pure Aero, the Rafa Origin does not put any dampening technology between player and ball, giving it the kind of direct ball feedback that advanced players crave.');

INSERT INTO product_image (product_id, image_name)¡@VALUES
(2, 'pure_aero_rafa_origin_1.png');

INSERT INTO product_image (product_id, image_name)¡@VALUES
(2, 'pure_aero_rafa_origin_2.png');

INSERT INTO product_image (product_id, image_name)¡@VALUES
(1, 'uso_balls_can_1.png');

INSERT INTO order_status (status_id, status_name) VALUES
(1, 'Awaiting review'),
(2, 'Reviewed'),
(3, 'Shipped');

INSERT INTO order_status (status_id, status_name) VALUES
(4, 'Cancelled')
