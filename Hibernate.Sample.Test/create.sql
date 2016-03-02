create table t_user(
  id bigint identity(1,1) primary key,
  name nvarchar(512),
  age int,
  group_id int
  );
  
  create table t_passport(
  id bigint primary key FOREIGN KEY REFERENCES t_user(id),
  serial varchar(30),
  expiry int
  );
  
  create table t_group(
  id bigint identity(1,1) primary key,
  name nvarchar(512),
  );