cd "C:\Program Files\PostgreSQL\17\bin"
echo $PATH
export PATH=$PATH:"C:\Program Files\PostgreSQL\17\bin"
echo $PATH

createdb -U "postgres" ynamar_engine_database;