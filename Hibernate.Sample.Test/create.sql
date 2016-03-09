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
  group_id bigint FOREIGN KEY REFERENCES t_group(id),
  role_id bigint FOREIGN KEY REFERENCES t_role(id)
);

create table t_book1(
  id bigint identity(1,1) primary key,
  name nvarchar(512),
  manufactrurer nvarchar(512),
  pagecount int
);

create table t_dvd1(
  id bigint identity(1,1) primary key,
  name nvarchar(512),
  manufactrurer nvarchar(512),
  regincode varchar(30)
);

create table t_item2(
  id bigint identity(1,1) primary key,
  name nvarchar(512),
  manufactrurer nvarchar(512)
);

create table t_book2(
  id bigint primary key FOREIGN KEY REFERENCES t_item2(id),
  pagecount int
);

create table t_dvd2(
  id bigint primary key FOREIGN KEY REFERENCES t_item2(id),
  regincode varchar(30)
);

create table t_item3(
  id bigint identity(1,1) primary key,
  name nvarchar(512),
  manufactrurer nvarchar(512),
  pagecount int,
  regincode varchar(30),
  category int
);
