CREATE SCHEMA point_of_sale;

CREATE USER point_of_sale WITH PASSWORD 'pass';
ALTER ROLE point_of_sale SET search_path = point_of_sale;
GRANT ALL ON SCHEMA point_of_sale TO point_of_sale;