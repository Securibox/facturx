namespace Securibox.FacturX.Schematron.Types
{
    /* param = element param {
     *   attribute name { nameValue },
     *   attribute value { non-empty-string }
     * }
     * Dear ISO, Why not just make param elements let elements? Why make an additional param type?
     */
    public class Param : Let { }
}
