namespace UserServices.Helpers {
	public static class ElementIdGenerator {
		public static string GetElementId(string propertyName,  int index) {
			return string.Format("{0}[{1}]", propertyName, index);
		}
		public static string GetDictionaryElementId(string propertyName, string key) {
			return string.Format("{0}[{1}]", propertyName, key);
		}
		public static string GetElementId(string containerName, string propertyName, int index) {
			return string.Format("{0}[{1}].{2}", containerName, index, propertyName);
		}

		public static string GetElementId(string containerName1, string containerName2, string propertyName, int index1, int index2) {
			return string.Format("{0}[{1}].{2}[{3}].{4}", containerName1, index1, containerName2, index2, propertyName);
		}
		
		public static string GetSanitizedElementId(string containerName, string propertyName, int index) {
			return GetElementId(containerName, propertyName, index).Replace('.', '_').Replace('[', '_').Replace(']', '_');
		}

		public static string GetSanitizedElementId(string containerName1, string containerName2, string propertyName, int index1, int index2) {
			return GetElementId(containerName1, containerName2, propertyName, index1, index2).Replace('.', '_').Replace('[', '_').Replace(']', '_');
		}
		
	}
}