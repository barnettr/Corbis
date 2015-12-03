using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Web.Compilation;
using System.Web.UI.WebControls;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using System.Reflection;

namespace Corbis.Web.Validation.Integration.AspNet
{
	/// <summary>
	/// Performs validation on a control's value using the validation specified on the property of <see cref="System.Type"/>.
	/// </summary>
	/// <remarks>
	/// </remarks>
	public class ParameterProxyValidator : BaseValidator
	{
		/// <summary>
		/// Determines whether the content in the input control is valid.
		/// </summary>
		/// <returns><see langword="true"/> if the control is valid; otherwise, <see langword="false"/>.</returns>
		protected override bool EvaluateIsValid()
		{
			ParameterInfo parameterToValidate = GetParameterToValidate();

			if (parameterToValidate != null)
			{
				Validator validator = ParameterValidatorFactory.CreateValidator(parameterToValidate);

				if (validator != null)
				{
					ValidationResults validationResults = validator.Validate(ConvertValue(parameterToValidate.ParameterType, this.GetControlValidationValue(this.ControlToValidate)));

					this.ErrorMessage = FormatErrorMessage(validationResults, this.DisplayMode);
					return validationResults.IsValid;
				}
			}

			this.ErrorMessage = "";
			return true;
		}

		internal static string FormatErrorMessage(ValidationResults results, ValidationSummaryDisplayMode displayMode)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string errorsListStart;
			string errorStart;
			string errorEnd;
			string errorListEnd;

			switch (displayMode)
			{
				case ValidationSummaryDisplayMode.List:
					errorsListStart = string.Empty;
					errorStart = string.Empty;
					errorEnd = "<br/>";
					errorListEnd = string.Empty;
					break;

				case ValidationSummaryDisplayMode.SingleParagraph:
					errorsListStart = string.Empty;
					errorStart = string.Empty;
					errorEnd = " ";
					errorListEnd = "<br/>";
					break;

				default:
					errorsListStart = "<ul>";
					errorStart = "<li>";
					errorEnd = "</li>";
					errorListEnd = "</ul>";
					break;
			}
			if (!results.IsValid)
			{
				stringBuilder.Append(errorsListStart);
				foreach (ValidationResult validationResult in results)
				{
					stringBuilder.Append(errorStart);
					stringBuilder.Append(validationResult.Message);
					stringBuilder.Append(errorEnd);
				}
				stringBuilder.Append(errorListEnd);
			}

			return stringBuilder.ToString();
		}

		private string sourceTypeName;
		/// <summary>
		/// Gets or sets the name of the type to use a source for validation specifications.
		/// </summary>
		public string SourceTypeName
		{
			get { return sourceTypeName; }
			set { sourceTypeName = value; }
		}

		private string operationName;
		/// <summary>
		/// Gets or sets the name of the operation to use as soource for validation specifications.
		/// </summary>
		public string OperationName
		{
			get { return operationName; }
			set { operationName = value; }
		}

		private string parameterName;
		/// <summary>
		/// Gets or sets the name of the parameter to use as soource for validation specifications.
		/// </summary>
		public string ParameterName
		{
			get { return parameterName; }
			set { parameterName = value; }
		}

		private string parameterTypes;
		/// <summary>
		/// Gets or sets the name of the parameter to use as soource for validation specifications.
		/// </summary>
		public string ParameterTypes
		{
			get { return parameterTypes; }
			set { parameterTypes = value; }
		}

		private ValidationSummaryDisplayMode displayMode;
		/// <summary>
		/// Gets or sets the <see cref="ValidationSummaryDisplayMode"/> indicating how to format multiple validation results.
		/// </summary>
		public ValidationSummaryDisplayMode DisplayMode
		{
			get { return displayMode; }
			set { displayMode = value; }
		}

		private ParameterInfo GetParameterToValidate()
		{
			ParameterInfo parameterToValidate = null;

			//Getting the parameter validator 
			if (!string.IsNullOrEmpty(this.OperationName) && !string.IsNullOrEmpty(this.ParameterName))
			{
				Type objectType = BuildManager.GetType(this.SourceTypeName, false);

				//Make sure it's a valid type
				if (objectType != null)
				{
					MethodInfo operation = null;

					//For overloaded operations, need parameter types too
					if (this.ParameterTypes == null)
					{
						operation = objectType.GetMethod(this.OperationName);
					}
					else
					{
						string[] paramterTypesStrings = this.ParameterTypes.Split("|".ToCharArray());
						if (paramterTypesStrings.Length > 0)
						{
							Type[] parameterTypes = new Type[paramterTypesStrings.Length];
							for (int i = 0; i < paramterTypesStrings.Length; i++)
							{
								parameterTypes[i] = BuildManager.GetType(paramterTypesStrings[i], false);
							}

							operation = objectType.GetMethod(this.OperationName, parameterTypes);
						}
					}

					//Make sure we have a valid methodInfo
					if (operation != null)
					{
						//Get the parameterinfo
						ParameterInfo[] operationParameters = operation.GetParameters();

						foreach (ParameterInfo parameter in operationParameters)
						{
							if (parameter.Name == this.ParameterName)
							{
								parameterToValidate = parameter;
								break;
							}
						}
					}
				}
			}

			return parameterToValidate;
		}

		private object ConvertValue(Type returnType, object value)
		{
			object returnValue = value;

			if (value != null && value.GetType() != returnType)
			{
				TypeConverter converter = TypeDescriptor.GetConverter(returnType);
				value = converter.ConvertFrom(null, CultureInfo.CurrentCulture, value);
			}

			return returnValue;
		}
	}
}