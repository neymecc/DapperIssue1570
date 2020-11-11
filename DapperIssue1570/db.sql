--#region function(s)

CREATE OR REPLACE FUNCTION utc_now()
    RETURNS TIMESTAMP WITH TIME ZONE
AS
$$
BEGIN
RETURN (now() AT TIME ZONE 'UTC');
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION increment_version()
    RETURNS TRIGGER AS
$$
BEGIN
    NEW.version = OLD.version + 1;
RETURN NEW;
END;
$$ language 'plpgsql';

--#endregion

--#region table(s)

CREATE TABLE role
(
    id   BIGSERIAL,
    name TEXT NOT NULL,

    PRIMARY KEY (id),
    UNIQUE (name)
);

INSERT INTO role (name)
VALUES ('User'),
       ('Supporter'),
       ('Moderator'),
       ('Administrator');

CREATE TABLE profile
(
    id                  BIGSERIAL,
    role_id             BIGINT    NOT NULL DEFAULT 1,
    username            TEXT      NOT NULL,
    email               TEXT      NOT NULL,
    full_name           TEXT      NULL,
    hash                TEXT      NOT NULL,
    salt                TEXT      NOT NULL,
    is_email_verified   BOOLEAN   NOT NULL DEFAULT FALSE,
    utc_register_date   TIMESTAMP NOT NULL DEFAULT utc_now(),
    utc_last_login_date TIMESTAMP NULL,
    email_verify_token  TEXT      NULL,
    version             BIGINT    NOT NULL DEFAULT 1,

    PRIMARY KEY (id),
    UNIQUE (username),
    UNIQUE (email)
);

CREATE TABLE refresh_token
(
    id              BIGSERIAL,
    profile_id      BIGINT    NOT NULL,
    token           TEXT      NOT NULL,
    utc_expire_date TIMESTAMP NOT NULL,

    PRIMARY KEY (id, profile_id),
    FOREIGN KEY (profile_id) REFERENCES profile (id) ON DELETE CASCADE
);

--#endregion

CREATE TRIGGER update_profile_version
    BEFORE UPDATE
    ON profile
    FOR EACH ROW
    EXECUTE PROCEDURE increment_version();


INSERT INTO profile (username, email, hash, salt, email_verify_token)
VALUES ('testUser1', 'test@gmail.com', 'testHash', 'testSalt', 'thisisemailatoken1'),
       ('testUser2', 'testemail@gmail.com', 'testHash213', 'testSal456654t', 'thisisatokeasddasn1');

INSERT INTO refresh_token (profile_id, token, utc_expire_date)
VALUES (1, 'thisisarersdadkljtoken', now()),
       (1, 'thisisarefreshtoken2', now()),
       (2, 'thisisat786okenasdqwhgj', now());