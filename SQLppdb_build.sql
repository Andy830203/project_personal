USE ppdb;

CREATE TABLE brand (
	brand_id int IDENTITY(1,1) NOT NULL,
	brand_name nvarchar(30) NOT NULL,
	CONSTRAINT brand_brand_id_pk PRIMARY KEY (brand_id)
)

CREATE TABLE category (
	category_id int IDENTITY(1,1) NOT NULL,
	category_name nvarchar(30) NOT NULL,
	CONSTRAINT category_category_id_pk PRIMARY KEY (category_id)
)

CREATE TABLE user_level (
	level_id  int NOT NULL,
	level_name nvarchar(30) NOT NULL,
	CONSTRAINT user_level_level_id_pk PRIMARY KEY (level_id)
)

CREATE TABLE user_status (
	status_id int NOT NULL,
	status_name nvarchar(30) NOT NULL
	CONSTRAINT user_status_status_id_pk PRIMARY KEY (status_id)
)

CREATE TABLE member (
	u_id int IDENTITY(1,1) NOT NULL,
	username varchar(30) NOT NULL,
	password varchar(30) NOT NULL,
	userlevel int NOT NULL,
	userstatus int NOT NULL,
	real_name nvarchar(50),
	phone nvarchar(30),
	email nvarchar(50),
	address nvarchar(50),
	birthday date,
	CONSTRAINT member_u_id_pk PRIMARY KEY (u_id),
	CONSTRAINT member_userlevel_fk FOREIGN KEY (userlevel) REFERENCES user_level (level_id),
	CONSTRAINT member_userstatus_fk FOREIGN KEY (userstatus) REFERENCES user_status (status_id),
)

ALTER TABLE member
ADD CONSTRAINT member_user_level_df DEFAULT 4 FOR userlevel;

ALTER TABLE member
ADD CONSTRAINT member_user_status_df DEFAULT 1 FOR userstatus;

CREATE TABLE order_status (
	status_id int NOT NULL,
	status_name nvarchar(30),
	CONSTRAINT order_status_order_id_pk PRIMARY KEY (status_id)
)


CREATE TABLE orders (
	order_id int IDENTITY(1,1) NOT NULL,
	buyer_id int NOT NULL,
	order_time datetime2,
	shipped_time datetime2,
	total_price int  NOT NULL,
	CONSTRAINT order_order_id_pk PRIMARY KEY (order_id),
	CONSTRAINT order_buyer_id_fk FOREIGN KEY (buyer_id) REFERENCES member (u_id)
)

ALTER TABLE orders
ADD status int
CONSTRAINT orders_status_fk FOREIGN KEY (status) REFERENCES order_status (status_id);

CREATE TABLE checkin (
	u_id int NOT NULL,
	c_date datetime2 NOT NULL DEFAULT GETDATE(),
	CONSTRAINT checkin_u_id_c_date_pk PRIMARY KEY (u_id, c_date),
	CONSTRAINT checkin_u_id_fk FOREIGN KEY (u_id) REFERENCES member (u_id)
)

CREATE TABLE size (
	s_id int NOT NULL,
	size_name nvarchar(30),
	CONSTRAINT size_s_id_pk PRIMARY KEY (s_id)
)

CREATE TABLE product (
	product_id int IDENTITY(1,1) NOT NULL,
	product_name nvarchar(50) NOT NULL, 
	brand int,
	category int,
	price int NOT NULL,
	on_shelf bit NOT NULL,
	description nvarchar(300),
	CONSTRAINT product_product_id_pk PRIMARY KEY (product_id),
	CONSTRAINT product_brand_fk FOREIGN KEY (brand) REFERENCES brand (brand_id),
	CONSTRAINT product_category_fk FOREIGN KEY (category) REFERENCES category (category_id)
)

CREATE TABLE product_image (
	image_id int IDENTITY(1,1) NOT NULL,
	product_id int NOT NULL,
	image_name nvarchar(300) NOT NULL,
	CONSTRAINT product_image_image_id_pk PRIMARY KEY (image_id),
	CONSTRAINT product_image_product_id_fk FOREIGN KEY (product_id) REFERENCES product (product_id)
)

CREATE TABLE size_product (
	ps_id int IDENTITY(1,1) NOT NULL,
	p_id int NOT NULL,
	s_id int,
	in_stock int NOT NULL,
	CONSTRAINT size_product_ps_id_pk PRIMARY KEY (ps_id),
	CONSTRAINT size_product_p_id_fk FOREIGN KEY (p_id) REFERENCES product (product_id),
	CONSTRAINT size_product_s_id_fk FOREIGN KEY (s_id) REFERENCES size(s_id)
)

CREATE TABLE order_item (
	order_item_id int IDENTITY(1,1) NOT NULL,
	order_id int,
	ps_id int,
	quantity int,
	unitprice int,
	CONSTRAINT order_item_order_item_id_pk PRIMARY KEY (order_item_id),
	CONSTRAINT order_item_order_id_fk FOREIGN KEY (order_id) REFERENCES orders (order_id),
	CONSTRAINT order_item_ps_id_fk FOREIGN KEY (ps_id) REFERENCES size_product (ps_id)
)

CREATE TABLE cart_item (
	ps_id int NOT NULL,
	u_id int NOT NULL,
	quantity int NOT NULL,
	CONSTRAINT cart_item_ps_id_fk FOREIGN KEY (ps_id) REFERENCES size_product (ps_id),
	CONSTRAINT cart_item_u_id_fk FOREIGN KEY (u_id) REFERENCES member (u_id)
)


/*
DROP TABLE checkin;
DROP TABLE product_image;
DROP TABLE brand;
DROP TABLE category;
DROP TABLE orders;
DROP TABLE product;
DROP TABLE user_level;
DROP TABLE user_status;
DROP TABLE member;
DROP TABLE order_item;
DROP TABLE size_product;
DROP TABLE size;
*/