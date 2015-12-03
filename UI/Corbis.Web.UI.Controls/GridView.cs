using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;

namespace Corbis.Web.UI.Controls
{

    /// <summary>

    /// Behavior added to a row of a gridview : select on click event

    /// </summary>

    public class GridView : System.Web.UI.WebControls.GridView
    {
        private bool selectable;

        public bool Selectable
        {
            get { return selectable; }
            set { selectable = value; }
        }


        protected override void OnRowCreated(GridViewRowEventArgs e)
        {
            base.OnRowCreated(e);

            if (Selectable && (e.Row.RowType == DataControlRowType.DataRow))
            {
                foreach (TableCell cell in e.Row.Cells)
                {
                    cell.Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(this, "Select$" + e.Row.RowIndex.ToString()));
                    cell.Style.Add(HtmlTextWriterStyle.Cursor, "pointer");
                    cell.Attributes.Add("title", "Select");
                }
            }

            if (MouseHoverRowHighlightEnabled)
            {

                // only apply changes if its DataRow

                if (e.Row.RowType == DataControlRowType.DataRow)
                {

                    // when mouse is over the row, save original color to new attribute

                    // and change it to highlight yellow color

                    e.Row.Attributes.Add("onmouseover",

                    string.Format("this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='{0}'",

                    ColorTranslator.ToHtml(RowHighlightColor)));



                    // when mouse leaves the row, change the bg color to its original value   

                    e.Row.Attributes.Add("onmouseout",

                    "this.style.backgroundColor=this.originalstyle;");

                }

            }

        }

        public bool MouseHoverRowHighlightEnabled
        {

            get
            {

                if (ViewState["MouseHoverRowHighlightEnabled"] != null)

                    return (bool)ViewState["MouseHoverRowHighlightEnabled"];

                else

                    return false;

            }

            set { ViewState["MouseHoverRowHighlightEnabled"] = value; }

        }

        public Color RowHighlightColor
        {

            get
            {

                if (ViewState["RowHighlightColor"] != null)

                    return (Color)ViewState["RowHighlightColor"];

                else
                {

                    // default color

                    return Color.Yellow;

                }

            }

            set { ViewState["RowHighlightColor"] = value; }



        }

    }

/*
    /// <summary>
    /// Summary description for ClickableGridView
    /// </summary>
    public class ClickableGridView : GridView
    {
        public string RowCssClass
        {
            get
            {
                string rowClass = (string)ViewState["rowClass"];
                if (!string.IsNullOrEmpty(rowClass))
                    return rowClass;
                else
                    return string.Empty;
            }
            set
            {
                ViewState["rowClass"] = value;
            }
        }

        public string HoverRowCssClass
        {
            get
            {
                string hoverRowClass = (string)ViewState["hoverRowClass"];
                if (!string.IsNullOrEmpty(hoverRowClass))
                    return hoverRowClass;
                else
                    return string.Empty;
            }
            set
            {
                ViewState["hoverRowClass"] = value;
            }
        }

        private static readonly object RowClickedEventKey = new object();

        public event GridViewRowClicked RowClicked;
        protected virtual void OnRowClicked(GridViewRowClickedEventArgs e)
        {
            if (RowClicked != null)
                RowClicked(this, e);
        }

        protected override void RaisePostBackEvent(string eventArgument)
        {
            if (eventArgument.StartsWith("rc"))
            {
                int index = Int32.Parse(eventArgument.Substring(2));
                GridViewRowClickedEventArgs args = new GridViewRowClickedEventArgs(Rows[index]);
                OnRowClicked(args);
            }
            else
                base.RaisePostBackEvent(eventArgument);
        }

        protected override void PrepareControlHierarchy()
        {
            base.PrepareControlHierarchy();

            for (int i = 0; i < Rows.Count; i++)
            {
                string argsData = "rc" + Rows[i].RowIndex.ToString();
                Rows[i].Attributes.Add("onclick", Page.ClientScript.GetPostBackEventReference(this, argsData));

                if (RowCssClass != string.Empty)
                    Rows[i].Attributes.Add("onmouseout", "this.className='" + RowCssClass + "';");

                if (HoverRowCssClass != string.Empty)
                    Rows[i].Attributes.Add("onmouseover", "this.className='" + HoverRowCssClass + "';");
            }
        }
    }

    public class GridViewRowClickedEventArgs : EventArgs
    {
        private GridViewRow _row;

        public GridViewRowClickedEventArgs(GridViewRow aRow)
            : base()
        {
            _row = aRow;
        }

        public GridViewRow Row
        {
            get
            { return _row; }
        }
    }

    public delegate void GridViewRowClicked(object sender, GridViewRowClickedEventArgs args);
 */
}