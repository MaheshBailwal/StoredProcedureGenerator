﻿using SPGenerator.DataModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SPGenerator.Core
{
    public abstract class BaseSPGenerator
    {
        #region Abstract Method
        protected abstract string GetSpName(string tableName);
        protected abstract void GenerateStatement(string tableName,StringBuilder sb, List<DBTableColumnInfo> selectedFields, List<DBTableColumnInfo> whereConditionFields);
        #endregion

        #region Static Members
        internal static string prefixWhereParameter;
        internal static string prefixInputParameter;
        internal static string prefixInsertSp;
        internal static string prefixUpdateSp;
        internal static bool errorHandling;
        internal static string[] sqlKeyWords;
        static BaseSPGenerator()
        {
            sqlKeyWords = File.ReadAllLines("SqlKeyWords.txt").Select(p => p.Trim().ToUpperInvariant()).ToArray();
        }
        public static void SetSettings(Settings setting)
        {
            prefixWhereParameter = setting.prefixWhereParameter;
            prefixInputParameter = setting.prefixInputParameter;
            prefixInsertSp = setting.prefixInsertSp;
            prefixUpdateSp = setting.prefixUpdateSp;
            errorHandling = setting.errorHandling == "Yes" ? true : false;
        }
        #endregion

        #region GenerateSP
        public void GenerateSp(string tableName, StringBuilder sb, List<DBTableColumnInfo> selectedFields, List<DBTableColumnInfo> whereConditionFields)
        {
            string spName = GetSpName(tableName);
            GenerateDropScript(spName, sb);
            sb.Append(Environment.NewLine + " CREATE PROCEDURE " + spName);
            GenerateErrorNumberOutParameter(sb);
            GenerateInputParameters(selectedFields, sb);
            GenerateWhereParameters(whereConditionFields, sb);
            sb.Append(Environment.NewLine + "AS" + Environment.NewLine + "BEGIN");
            GenerateStartTryBlock(sb);
            GenerateStatement(tableName,sb, selectedFields, whereConditionFields);
            GenerateEndTryBlock(sb);
            GenerateCatchBlock(sb);
            sb.Append(Environment.NewLine + "END");
            sb.Append(Environment.NewLine + "GO");
            sb.Append(Environment.NewLine);
        }

        protected virtual void GenerateDropScript(string spName, StringBuilder sb)
        {
            sb.Append(Environment.NewLine + "IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'");
            sb.Append(spName);
            sb.Append("')AND type in (N'P', N'PC'))");
            sb.Append(Environment.NewLine + "DROP PROCEDURE ");
            sb.Append(spName);
            sb.Append(Environment.NewLine + "GO" + Environment.NewLine);
        }

        protected virtual void GenerateInputParameters(List<DBTableColumnInfo> tableFields, StringBuilder sb)
        {
            foreach (DBTableColumnInfo colInf in tableFields)
            {
                if (colInf.Exclude)
                    continue;
                sb.Append(Environment.NewLine + prefixInputParameter + colInf.ColumnName);
                sb.Append(" " + colInf.DataType);
                if (colInf.CharacterMaximumLength > 0)
                {
                    sb.Append("(" + colInf.CharacterMaximumLength.ToString() + ")");
                }
                sb.Append(",");
            }
            //Remove Commma from end
            sb[sb.Length - 1] = ' ';
        }

        protected void GenerateWhereParameters(List<DBTableColumnInfo> whereConditionFields, StringBuilder sb)
        {
            sb.Append(",");
            foreach (DBTableColumnInfo colInf in whereConditionFields)
            {
                sb.Append(Environment.NewLine + prefixWhereParameter + colInf.ColumnName);
                sb.Append(" " + colInf.DataType);
                if (colInf.CharacterMaximumLength > 0)
                {
                    sb.Append("(" + colInf.CharacterMaximumLength.ToString() + ")");
                }
                sb.Append(",");
            }
            //Remove Commma from end
            sb[sb.Length - 1] = ' ';
        }

        protected void GenerateWhereStatement(List<DBTableColumnInfo> whereConditionFields, StringBuilder sb)
        {
            sb.Append(Environment.NewLine + "\tWHERE ");
            foreach (DBTableColumnInfo colInf in whereConditionFields)
            {
                sb.Append(colInf.ColumnName + "=" + prefixWhereParameter + colInf.ColumnName);
                sb.Append(" AND ");
            }
            sb.Remove(sb.Length - 5, 5);
        }

        #region ErrorHandling
        private void GenerateStartTryBlock(StringBuilder sb)
        {
            if (!errorHandling)
                return;
            sb.Append(Environment.NewLine + "BEGIN TRY");
        }

        private void GenerateEndTryBlock(StringBuilder sb)
        {
            if (!errorHandling)
                return;
            sb.Append(Environment.NewLine + "END TRY");
        }
        private void GenerateErrorNumberOutParameter(StringBuilder sb)
        {
            if (!errorHandling)
                return;
            sb.Append(Environment.NewLine + "@out_error_number INT = 0 OUTPUT,");
        }

        private void GenerateCatchBlock(StringBuilder sb)
        {
            if (!errorHandling)
                return;
            sb.Append(Environment.NewLine + "BEGIN CATCH");
            sb.Append(Environment.NewLine + "\tSELECT @out_error_number=ERROR_NUMBER()");
            sb.Append(Environment.NewLine + "END CATCH");
        }
        #endregion

        protected string WrapIfKeyWord(string name)
        {
            if (sqlKeyWords.Contains(name.Trim().ToUpperInvariant()))
            {
                name = "[" + name + "]";
            }
            return name;
        }
        #endregion
    }
}
