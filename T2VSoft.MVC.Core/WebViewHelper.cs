using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace T2VSoft.MVC.Core
{
    public static class CheckBoxExtensions
    {
        public static MvcHtmlString LabeledCheckBoxFor<TModel>(
            this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, bool>> expression, string labelText = null,
            object labelHtmlAttributes = null,
            object checkboxHtmlAttributes = null)
        {
            var modelMetadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            string htmlFieldName = ExpressionHelper.GetExpressionText(expression);
            var labelAttrs = HtmlHelper.AnonymousObjectToHtmlAttributes(labelHtmlAttributes);
            var checkboxAttrs = HtmlHelper.AnonymousObjectToHtmlAttributes(checkboxHtmlAttributes);

            var str = labelText
                ?? (modelMetadata.DisplayName
                ?? (modelMetadata.PropertyName
                ?? htmlFieldName.Split(new[] { '.' }).Last()));

            var labelTag = new TagBuilder("label");
            if (labelAttrs != null)
                labelTag.MergeAttributes(labelAttrs);

            labelTag.InnerHtml = htmlHelper.CheckBoxFor(expression, checkboxAttrs) + str;
            return MvcHtmlString.Create(labelTag.ToString(TagRenderMode.Normal));
        }


    }

    public static class LabelExtensions
    {
        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            return html.LabelFor(expression, null, htmlAttributes);
        }

        public static MvcHtmlString LabelFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, string labelText, object htmlAttributes)
        {
            return html.LabelHelper(
                ModelMetadata.FromLambdaExpression(expression, html.ViewData),
                ExpressionHelper.GetExpressionText(expression),
                HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes),
                labelText);
        }

        private static MvcHtmlString LabelHelper(this HtmlHelper html, ModelMetadata metadata, string htmlFieldName, IDictionary<string, object> htmlAttributes, string labelText = null)
        {
            var str = labelText
                ?? (metadata.DisplayName
                ?? (metadata.PropertyName
                ?? htmlFieldName.Split(new[] { '.' }).Last()));

            if (string.IsNullOrEmpty(str))
                return MvcHtmlString.Empty;

            var tagBuilder = new TagBuilder("label");
            tagBuilder.MergeAttributes(htmlAttributes);
            tagBuilder.Attributes.Add("for", TagBuilder.CreateSanitizedId(html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(htmlFieldName)));
            tagBuilder.SetInnerText(str);

            return tagBuilder.ToMvcHtmlString(TagRenderMode.Normal);
        }

        private static MvcHtmlString ToMvcHtmlString(this TagBuilder tagBuilder, TagRenderMode renderMode)
        {
            return new MvcHtmlString(tagBuilder.ToString(renderMode));
        }
    }

    public static class PasswordExtensions
    {

        public static MvcHtmlString RevealedPasswordFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> expression, object htmlAttributes)
        {
            var model = html.ViewData.Model;
            var tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
            var mm = ModelMetadata.FromLambdaExpression(expression, html.ViewData);
            var name = TagBuilder.CreateSanitizedId(
                html.ViewContext.ViewData.TemplateInfo.GetFullHtmlFieldName(
                    ExpressionHelper.GetExpressionText(expression)));
            tagBuilder.Attributes.Add("name", name
                                  );
            tagBuilder.Attributes.Add("type", "password");

            tagBuilder.MergeAttributes(html.GetUnobtrusiveValidationAttributes(name, mm));
            if (model != null)
            {
                var value = expression.Compile().Invoke(model);
                if(value != null)
                    tagBuilder.Attributes.Add("value", value.ToString());
            }
            return tagBuilder.ToMvcHtmlString(TagRenderMode.Normal);
        }

        private static MvcHtmlString ToMvcHtmlString(this TagBuilder tagBuilder, TagRenderMode renderMode)
        {
            return new MvcHtmlString(tagBuilder.ToString(renderMode));
        }
    } 

}