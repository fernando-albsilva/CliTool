using CliTool.Modules.OracleExemple;

namespace CliTool.Modules.Configuration.Payload
{
    public static class OracleSnippetPayload
    {
        public static List<SnippetArg> Snippets => new List<SnippetArg>
        {
            new SnippetArg
            {
                Title = "Buscar tabela pelo nome (usando ALL_TABLES)",
                Snippet = @"
SELECT OWNER, TABLE_NAME 
FROM ALL_TABLES 
WHERE TABLE_NAME LIKE UPPER('%:tableName%')
ORDER BY OWNER, TABLE_NAME;"
            },

            new SnippetArg
            {
                Title = "Buscar colunas por nome em todas as tabelas (ALL_TAB_COLUMNS)",
                Snippet = @"
SELECT OWNER, TABLE_NAME, COLUMN_NAME, DATA_TYPE, DATA_LENGTH 
FROM ALL_TAB_COLUMNS 
WHERE COLUMN_NAME LIKE UPPER('%:columnName%')
ORDER BY OWNER, TABLE_NAME;"
            },

            new SnippetArg
            {
                Title = "Listar todas as constraints de uma tabela",
                Snippet = @"
SELECT CONSTRAINT_NAME, CONSTRAINT_TYPE, STATUS 
FROM ALL_CONSTRAINTS 
WHERE TABLE_NAME = UPPER(':tableName');"
            },

            new SnippetArg
            {
                Title = "Listar chaves estrangeiras de uma tabela",
                Snippet = @"
SELECT a.CONSTRAINT_NAME, a.TABLE_NAME, a.R_CONSTRAINT_NAME, b.TABLE_NAME AS REFERENCED_TABLE
FROM ALL_CONSTRAINTS a
JOIN ALL_CONSTRAINTS b ON a.R_CONSTRAINT_NAME = b.CONSTRAINT_NAME
WHERE a.CONSTRAINT_TYPE = 'R'
  AND a.TABLE_NAME = UPPER(':tableName');"
            },

            new SnippetArg
            {
                Title = "Listar índices de uma tabela",
                Snippet = @"
SELECT INDEX_NAME, TABLE_NAME, UNIQUENESS
FROM ALL_INDEXES
WHERE TABLE_NAME = UPPER(':tableName');"
            },

            new SnippetArg
            {
                Title = "Ver definição de uma sequence",
                Snippet = @"
SELECT SEQUENCE_NAME, MIN_VALUE, MAX_VALUE, INCREMENT_BY, LAST_NUMBER
FROM ALL_SEQUENCES
WHERE SEQUENCE_NAME = UPPER(':sequenceName');"
            },

            new SnippetArg
            {
                Title = "Ver triggers de uma tabela",
                Snippet = @"
SELECT TRIGGER_NAME, TRIGGER_TYPE, TRIGGERING_EVENT, STATUS
FROM ALL_TRIGGERS
WHERE TABLE_NAME = UPPER(':tableName');"
            },

            new SnippetArg
            {
                Title = "Buscar view por nome (ALL_VIEWS)",
                Snippet = @"
SELECT OWNER, VIEW_NAME, TEXT_LENGTH
FROM ALL_VIEWS
WHERE VIEW_NAME LIKE UPPER('%:viewName%')
ORDER BY OWNER;"
            },

            new SnippetArg
            {
                Title = "Ver código de uma view",
                Snippet = @"
SELECT TEXT
FROM ALL_VIEWS
WHERE VIEW_NAME = UPPER(':viewName');"
            },

            new SnippetArg
            {
                Title = "Buscar procedure ou function por nome",
                Snippet = @"
SELECT OWNER, OBJECT_NAME, OBJECT_TYPE, STATUS
FROM ALL_OBJECTS
WHERE OBJECT_TYPE IN ('PROCEDURE', 'FUNCTION')
  AND OBJECT_NAME LIKE UPPER('%:procName%');"
            },

            new SnippetArg
            {
                Title = "Mostrar tablespaces e tamanhos",
                Snippet = @"
SELECT TABLESPACE_NAME, STATUS, CONTENTS
FROM DBA_TABLESPACES;"
            },
        };
    }
}
