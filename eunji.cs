using System;
using System.Collections.Generic;
using System.Linq;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

[assembly: CommandClass(typeof(eunji.Commands))]
namespace eunji
{
    public class Commands
    {
        private bool GetTargetPL(ref Document acDoc, ref Database acCurdb, ref Editor acEd, ref Polyline targetPL, ref Point3d pickedPoint)
        {
            using (Transaction acTrans = acCurdb.TransactionManager.StartTransaction())
            {
                var options = new PromptEntityOptions("\nSelect a polyline: ");
                options.SetRejectMessage("\nSelected object is invalid.");
                options.AddAllowedClass(typeof(Polyline), true);
                var result = acEd.GetEntity(options);
                if (result.Status != PromptStatus.OK)
                    return false;
                else
                {
                    targetPL = (Polyline)acTrans.GetObject(result.ObjectId, OpenMode.ForRead);
                    pickedPoint = result.PickedPoint;
                    return true;
                }
            }
        }

        [CommandMethod("DV")]
        public void DV()
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            Editor acEd = acDoc.Editor;
            Polyline targetPL = new Polyline();
            Point3d pickedPoint = new Point3d();
            if (GetTargetPL(ref acDoc, ref acCurDb, ref acEd, ref targetPL, ref pickedPoint))
            {
                GetDIVVERTResult(ref acDoc, ref acCurDb, ref targetPL, ref pickedPoint);
            }
        }

        private void GetDIVVERTResult(ref Document acDoc, ref Database acCurdb, ref Polyline targetPL, ref Point3d pickedPoint)
        {
            DBObjectCollection acDBObjColl = new DBObjectCollection();
            acDBObjColl.Add(targetPL);
            DBObjectCollection targetPLRegionColl = Region.CreateFromCurves(acDBObjColl);
            Region targetPLRegion = targetPLRegionColl[0] as Region;

            List<double> xList = new List<double>();
            List<double> yList = new List<double>();
            for (int i = 1; i < targetPL.NumberOfVertices; i++)
            {
                xList.Add(targetPL.GetPoint2dAt(i).X);
                yList.Add(targetPL.GetPoint2dAt(i).Y);
            }

            double targetXmin = xList.Min();
            double targetXmax = xList.Max();
            double targetYmax = yList.Max();
            double targetYmin = yList.Min();
            double tempY = (targetYmax + targetYmin) / 2;
            double lowerbound = targetYmin;
            double upperbound = targetYmax;
            double tempPLRegionArea = 0;

            PromptStringOptions pStrOpts = new PromptStringOptions("\nEnter the area you want: ");
            pStrOpts.AllowSpaces = false;
            double targetArea = double.Parse(acDoc.Editor.GetString(pStrOpts).StringResult) * 1000000;
            if (targetArea > targetPL.Area)
            {
                Application.ShowAlertDialog("Target Area is larger than selected Polyline's area!");
                return;
            }
            if (pickedPoint.Y < tempY)
            {
                while (true)
                {
                    Polyline tempPL = new Polyline();
                    tempPL.AddVertexAt(0, new Point2d(targetXmin, targetYmin), 0, 0, 0);
                    tempPL.AddVertexAt(1, new Point2d(targetXmin, tempY), 0, 0, 0);
                    tempPL.AddVertexAt(2, new Point2d(targetXmax, tempY), 0, 0, 0);
                    tempPL.AddVertexAt(3, new Point2d(targetXmax, targetYmin), 0, 0, 0);
                    tempPL.AddVertexAt(4, new Point2d(targetXmin, targetYmin), 0, 0, 0);
                    tempPL.Closed = true;

                    DBObjectCollection acDBObjColl2 = new DBObjectCollection();
                    acDBObjColl2.Add(tempPL);
                    DBObjectCollection tempPLRegionColl = Region.CreateFromCurves(acDBObjColl2);
                    Region tempPLRegion = tempPLRegionColl[0] as Region;
                    tempPLRegion.BooleanOperation(BooleanOperationType.BoolIntersect, (Region)targetPLRegion.Clone());
                    tempPLRegionArea = tempPLRegion.Area;
                    if (Math.Abs(tempPLRegionArea - targetArea) < 100)
                    {
                        break;
                    }
                    else if (tempPLRegionArea > targetArea)
                    {
                        upperbound = tempY;
                        tempY = (tempY + lowerbound) / 2;
                    }
                    else
                    {
                        lowerbound = tempY;
                        tempY = (tempY + upperbound) / 2;
                    }
                }
            }
            else if (pickedPoint.Y > tempY)
            {
                while (true)
                {
                    Polyline tempPL = new Polyline();
                    tempPL.AddVertexAt(0, new Point2d(targetXmin, targetYmax), 0, 0, 0);
                    tempPL.AddVertexAt(1, new Point2d(targetXmin, tempY), 0, 0, 0);
                    tempPL.AddVertexAt(2, new Point2d(targetXmax, tempY), 0, 0, 0);
                    tempPL.AddVertexAt(3, new Point2d(targetXmax, targetYmax), 0, 0, 0);
                    tempPL.AddVertexAt(4, new Point2d(targetXmin, targetYmax), 0, 0, 0);
                    tempPL.Closed = true;

                    DBObjectCollection acDBObjColl2 = new DBObjectCollection();
                    acDBObjColl2.Add(tempPL);
                    DBObjectCollection tempPLRegionColl = Region.CreateFromCurves(acDBObjColl2);
                    Region tempPLRegion = tempPLRegionColl[0] as Region;
                    tempPLRegion.BooleanOperation(BooleanOperationType.BoolIntersect, (Region)targetPLRegion.Clone());
                    tempPLRegionArea = tempPLRegion.Area;
                    if (Math.Abs(tempPLRegionArea - targetArea) < 10)
                    {
                        break;
                    }
                    else if (tempPLRegionArea > targetArea)
                    {
                        lowerbound = tempY;
                        tempY = (tempY + upperbound) / 2;
                    }
                    else
                    {
                        upperbound = tempY;
                        tempY = (tempY + lowerbound) / 2;
                    }
                }
            }
            else
            {
                return;
            }
            Xline tempLine = new Xline();
            tempLine.BasePoint = new Point3d(0, tempY, 0);
            tempLine.UnitDir = new Vector3d(1, 0, 0);
            Point3dCollection intersectPoints = new Point3dCollection();
            tempLine.IntersectWith(targetPL, Intersect.OnBothOperands, intersectPoints, IntPtr.Zero, IntPtr.Zero);
            using (Transaction acTrans = acCurdb.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurdb.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                using (Line acLine = new Line(intersectPoints[0], intersectPoints[1]))
                {
                    acBlkTblRec.AppendEntity(acLine);
                    acTrans.AddNewlyCreatedDBObject(acLine, true);
                }
                acTrans.Commit();
            }
        }

        [CommandMethod("DH")]
        public void DH()
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            Editor acEd = acDoc.Editor;
            Polyline targetPL = new Polyline();
            Point3d pickedPoint = new Point3d();
            if (GetTargetPL(ref acDoc, ref acCurDb, ref acEd, ref targetPL, ref pickedPoint))
            {
                GetDIVHORIZResult(ref acDoc, ref acCurDb, ref targetPL, ref pickedPoint);
            }
        }

        private void GetDIVHORIZResult(ref Document acDoc, ref Database acCurdb, ref Polyline targetPL, ref Point3d pickedPoint)
        {
            DBObjectCollection acDBObjColl = new DBObjectCollection();
            acDBObjColl.Add(targetPL);
            DBObjectCollection targetPLRegionColl = Region.CreateFromCurves(acDBObjColl);
            Region targetPLRegion = targetPLRegionColl[0] as Region;

            List<double> xList = new List<double>();
            List<double> yList = new List<double>();
            for (int i = 1; i < targetPL.NumberOfVertices; i++)
            {
                xList.Add(targetPL.GetPoint2dAt(i).X);
                yList.Add(targetPL.GetPoint2dAt(i).Y);
            }

            double targetXmin = xList.Min();
            double targetXmax = xList.Max();
            double targetYmax = yList.Max();
            double targetYmin = yList.Min();
            double tempX = (targetXmax + targetXmin) / 2;
            double leftbound = targetXmin;
            double rightbound = targetXmax;
            double tempPLRegionArea = 0;

            PromptStringOptions pStrOpts = new PromptStringOptions("\nEnter the area you want: ");
            pStrOpts.AllowSpaces = false;
            double targetArea = double.Parse(acDoc.Editor.GetString(pStrOpts).StringResult) * 1000000;
            if (targetArea > targetPL.Area)
            {
                Application.ShowAlertDialog("Target Area is larger than selected Polyline's area!");
                return;
            }
            if (pickedPoint.X < tempX)
            {
                while (true)
                {
                    Polyline tempPL = new Polyline();
                    tempPL.AddVertexAt(0, new Point2d(targetXmin, targetYmax), 0, 0, 0);
                    tempPL.AddVertexAt(1, new Point2d(tempX, targetYmax), 0, 0, 0);
                    tempPL.AddVertexAt(2, new Point2d(tempX, targetYmin), 0, 0, 0);
                    tempPL.AddVertexAt(3, new Point2d(targetXmin, targetYmin), 0, 0, 0);
                    tempPL.AddVertexAt(4, new Point2d(targetXmin, targetYmax), 0, 0, 0);
                    tempPL.Closed = true;

                    DBObjectCollection acDBObjColl2 = new DBObjectCollection();
                    acDBObjColl2.Add(tempPL);
                    DBObjectCollection tempPLRegionColl = Region.CreateFromCurves(acDBObjColl2);
                    Region tempPLRegion = tempPLRegionColl[0] as Region;
                    tempPLRegion.BooleanOperation(BooleanOperationType.BoolIntersect, (Region)targetPLRegion.Clone());
                    tempPLRegionArea = tempPLRegion.Area;
                    if (Math.Abs(tempPLRegionArea - targetArea) < 10)
                    {
                        break;
                    }
                    else if (tempPLRegionArea > targetArea)
                    {
                        rightbound = tempX;
                        tempX = (tempX + leftbound) / 2;
                    }
                    else
                    {
                        leftbound = tempX;
                        tempX = (tempX + rightbound) / 2;
                    }
                }
            }
            else if (pickedPoint.X > tempX)
            {
                while (true)
                {
                    Polyline tempPL = new Polyline();
                    tempPL.AddVertexAt(0, new Point2d(targetXmax, targetYmax), 0, 0, 0);
                    tempPL.AddVertexAt(1, new Point2d(targetXmax, targetYmin), 0, 0, 0);
                    tempPL.AddVertexAt(2, new Point2d(tempX, targetYmin), 0, 0, 0);
                    tempPL.AddVertexAt(3, new Point2d(tempX, targetYmax), 0, 0, 0);
                    tempPL.AddVertexAt(4, new Point2d(targetXmax, targetYmax), 0, 0, 0);
                    tempPL.Closed = true;

                    DBObjectCollection acDBObjColl2 = new DBObjectCollection();
                    acDBObjColl2.Add(tempPL);
                    DBObjectCollection tempPLRegionColl = Region.CreateFromCurves(acDBObjColl2);
                    Region tempPLRegion = tempPLRegionColl[0] as Region;
                    tempPLRegion.BooleanOperation(BooleanOperationType.BoolIntersect, (Region)targetPLRegion.Clone());
                    tempPLRegionArea = tempPLRegion.Area;
                    if (Math.Abs(tempPLRegionArea - targetArea) < 10000)
                    {
                        break;
                    }
                    else if (tempPLRegionArea > targetArea)
                    {
                        leftbound = tempX;
                        tempX = (tempX + rightbound) / 2;
                    }
                    else
                    {
                        rightbound = tempX;
                        tempX = (tempX + leftbound) / 2;
                    }
                }
            }
            else
            {
                return;
            }
            Xline tempLine = new Xline();
            tempLine.BasePoint = new Point3d(tempX, 0, 0);
            tempLine.UnitDir = new Vector3d(0, 1, 0);
            Point3dCollection intersectPoints = new Point3dCollection();
            tempLine.IntersectWith(targetPL, Intersect.OnBothOperands, intersectPoints, IntPtr.Zero, IntPtr.Zero);
            using (Transaction acTrans = acCurdb.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurdb.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                using (Line acLine = new Line(intersectPoints[0], intersectPoints[1]))
                {
                    acBlkTblRec.AppendEntity(acLine);
                    acTrans.AddNewlyCreatedDBObject(acLine, true);
                }
                acTrans.Commit();
            }
        }


        [CommandMethod("CCX")]
        public void CCX()
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            Editor acEd = acDoc.Editor;

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                var options = new PromptEntityOptions("\nSelect first line or arc: ");
                options.SetRejectMessage("\nSelected object is invalid.");
                options.AddAllowedClass(typeof(Line), true);
                options.AddAllowedClass(typeof(Arc), true);
                var result = acEd.GetEntity(options);
                if (result.Status != PromptStatus.OK)
                {
                    Application.ShowAlertDialog("Please select a line or arc.");
                    return;
                }

                if (result.GetType() == typeof(Line))
                {
                    Line firstline = (Line)acTrans.GetObject(result.ObjectId, OpenMode.ForRead);
                    Line secondline = new Line();
                    options = new PromptEntityOptions("\nSelect second line: ");
                    options.SetRejectMessage("\nSelected object is invalid.");
                    options.AddAllowedClass(typeof(Line), true);
                    result = acEd.GetEntity(options);
                    if (result.Status != PromptStatus.OK)
                    {
                        Application.ShowAlertDialog("Please select another line.");
                        return;
                    }
                    else
                    {
                        secondline = (Line)acTrans.GetObject(result.ObjectId, OpenMode.ForRead);
                    }

                    Vector2d firstlineDirVec = new Vector2d(Math.Abs(firstline.EndPoint.X - firstline.StartPoint.X) < 0.0001 ? 0 : firstline.EndPoint.X - firstline.StartPoint.X, Math.Abs(firstline.EndPoint.Y - firstline.StartPoint.Y) < 0.0001 ? 0 : firstline.EndPoint.Y - firstline.StartPoint.Y);
                    Vector2d secondlineDirVec = new Vector2d(Math.Abs(secondline.EndPoint.X - secondline.StartPoint.X) < 0.0001 ? 0 : secondline.EndPoint.X - secondline.StartPoint.X, Math.Abs(secondline.EndPoint.Y - secondline.StartPoint.Y) < 0.0001 ? 0 : secondline.EndPoint.Y - secondline.StartPoint.Y);
                    if (!firstlineDirVec.IsParallelTo(secondlineDirVec))
                    {
                        Application.ShowAlertDialog("The lines are not parallel!");
                        return;
                    }
                    BlockTable acBlkTbl;
                    acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                    OpenMode.ForRead) as BlockTable;
                    BlockTableRecord acBlkTblRec;
                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                    OpenMode.ForWrite) as BlockTableRecord;
                    Xline middleline = new Xline();
                    middleline.BasePoint = new Point3d((firstline.EndPoint.X + secondline.EndPoint.X) / 2, (firstline.EndPoint.Y + secondline.EndPoint.Y) / 2, 0);
                    middleline.UnitDir = new Vector3d(Math.Abs(firstline.EndPoint.X - firstline.StartPoint.X) < 0.0001 ? 0 : firstline.EndPoint.X - firstline.StartPoint.X, Math.Abs(firstline.EndPoint.Y - firstline.StartPoint.Y) < 0.0001 ? 0 : firstline.EndPoint.Y - firstline.StartPoint.Y, 0);
                    acBlkTblRec.AppendEntity(middleline);
                    acTrans.AddNewlyCreatedDBObject(middleline, true);
                    acTrans.Commit();
                }
                else
                {
                    Arc firstarc = (Arc)acTrans.GetObject(result.ObjectId, OpenMode.ForRead);
                    Arc secondarc = new Arc();
                    options = new PromptEntityOptions("\nSelect second arc: ");
                    options.SetRejectMessage("\nSelected object is invalid.");
                    options.AddAllowedClass(typeof(Arc), true);
                    result = acEd.GetEntity(options);
                    if (result.Status != PromptStatus.OK)
                    {
                        Application.ShowAlertDialog("Please select another arc.");
                        return;
                    }
                    else
                    {
                        secondarc = (Arc)acTrans.GetObject(result.ObjectId, OpenMode.ForRead);
                    }

                    if (!firstarc.Center.IsEqualTo(secondarc.Center))
                    {
                        Application.ShowAlertDialog("The centers are not same!");
                        return;
                    }
                    BlockTable acBlkTbl;
                    acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId,
                                                    OpenMode.ForRead) as BlockTable;
                    BlockTableRecord acBlkTblRec;
                    acBlkTblRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace],
                                                    OpenMode.ForWrite) as BlockTableRecord;

                    Arc middlearc = new Arc();
                    middlearc.Center = firstarc.Center;
                    middlearc.Radius = (firstarc.Radius + secondarc.Radius) / 2;
                    middlearc.StartAngle = firstarc.StartAngle < secondarc.StartAngle ? firstarc.StartAngle : secondarc.StartAngle;
                    middlearc.EndAngle = firstarc.EndAngle > secondarc.EndAngle ? firstarc.EndAngle : secondarc.EndAngle;
                    acBlkTblRec.AppendEntity(middlearc);
                    acTrans.AddNewlyCreatedDBObject(middlearc, true);
                    acTrans.Commit();
                }
            }
        }
    }
}
