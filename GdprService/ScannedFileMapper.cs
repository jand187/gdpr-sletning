﻿using System;
using System.Threading.Tasks;

namespace GdprService
{
	public interface IScannedFileMapper
	{
		Task<ScannedFile> Map(string line);
	}

	public class ScannedFileMapper : IScannedFileMapper
	{
		private readonly ILogger logger;

		public ScannedFileMapper(ILogger logger)
		{
			this.logger = logger;
		}

		public async Task<ScannedFile> Map(string line)
		{
			var fields = line.Split(';');

			return await Task.Run(() => CreateScannedFile(fields));
		}

		private ScannedFile CreateScannedFile(string[] fields)
		{
			var scannedFile = new ScannedFile
			{
				Repository = fields[(int) CsvFields.Repository],
				Filename = fields[(int) CsvFields.FileName],
				Status = fields[(int) CsvFields.Status],
				Comment = fields[(int) CsvFields.Comment]
			};
			var dateField = fields[(int) CsvFields.LastModified];
			if (DateTime.TryParse(dateField, out var result))
			{
				scannedFile.LastModified = result;
			}
			else
			{
				logger.Log($"File: '{scannedFile.Filename}' did not have a LastModified date in the scan result.");
			}

			return scannedFile;
		}
	}

	public enum CsvFields
	{
		Repository = 0,
		FileName = 1,
		Status = 2,
		Comment = 3,
		CurrentLabel = 4,
		CurrentLabelId = 5,
		AppliedLabel = 6,
		AppliedLabelId = 7,
		ConditionName = 8,
		MatchedString = 9,
		InformationTypeName = 10,
		MatchedInformationTypesString = 11,
		Action = 12,
		LastModified = 13,
		LastModifiedBy = 14,
		ProtectionBeforeAction = 15,
		ProtectionAfterAction = 16
	}
}
