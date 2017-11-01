/**
 * Company:Fansasy
 * Useage：为Controller增加分组描述信息
 * For:Swagger文档
 * **/
using System;

namespace OrchardSwagger.Swagger.Annotations
{
    /// <summary>
    /// Controller描述信息
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class SwaggerControllerGroupAttribute : Attribute
    {
        /// <summary>
        /// 当前Controller所属模块 请用中文
        /// </summary>
        public string GroupName { get; private set; }

        /// <summary>
        /// 当前controller用途    请用中文
        /// </summary>
        public string Useage { get; private set; }

        /// <summary>
        ///  Controller描述信息 构造
        /// </summary>
        /// <param name="groupName">所属模块名称</param>
        /// <param name="useage">当前controller用途</param>
        public SwaggerControllerGroupAttribute(string groupName, string useage)
        {
            if (string.IsNullOrEmpty(groupName) || string.IsNullOrEmpty(useage))
            {
                throw new ArgumentNullException("groupName||useage");
            }
            GroupName = groupName;
            Useage = useage;
        }
    }
}
