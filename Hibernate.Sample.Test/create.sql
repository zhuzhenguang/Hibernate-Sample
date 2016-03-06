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
  name nvarchar(512)
  );

  alter table t_address alter column user_id bigint not null;

  create table t_role(
  id bigint identity(1,1) primary key,
  name nvarchar(512)
  );

  create table t_group_role(
  group_id bigint,
  role_id bigint
  );