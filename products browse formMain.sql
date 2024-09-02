-- join product_image table and only show the first picture
SELECT p.*, b.brand_name, c.category_name, i.image_name 
  FROM product AS p 
  JOIN brand AS b 
  ON p.brand = b.brand_id 
  JOIN category AS c 
  ON p.category = c.category_id 
  JOIN (SELECT *, ROW_NUMBER() OVER (PARTITION BY product_id ORDER BY image_id) AS rowNum
		FROM product_image 
		) AS i 
  ON p.product_id = i.product_id 
  WHERE on_shelf = 1 AND rowNum = 1;



WITH CTE AS (
	SELECT *, ROW_NUMBER() OVER (PARTITION BY product_id ORDER BY image_id) AS rowNum
	FROM product_image 
)
SELECT image_id, product_id, image_name
FROM CTE
WHERE rowNum = 1;

SELECT p.*, b.brand_name, c.category_name, i.image_name FROM product AS p JOIN brand AS b ON p.brand = b.brand_id JOIN category AS c ON p.category = c.category_id JOIN (SELECT *, ROW_NUMBER() OVER (PARTITION BY product_id ORDER BY image_id) AS rowNum FROM product_image) AS i ON p.product_id = i.product_id WHERE on_shelf = 1 AND rowNum = 1;
