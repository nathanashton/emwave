   M S B A M L   `   `   `  ����J  FWpfApplication2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:WpfApplication2.CustomControlsWpfApplication2.CustomControls  5   	        WpfApplication2.MainWindow   5   	   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   b ^Syncfusion.SfChart.WPF, Version=14.1451.0.41, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89b ^Syncfusion.SfGauge.WPF, Version=14.1451.0.41, Culture=neutral, PublicKeyToken=3d67ed1f87d44c89/chart!http://schemas.syncfusion.com/wpf   5   	   CcustomControls,clr-namespace:WpfApplication2.CustomControls   5   	   -   5        $
��Main��5        � Title$  
MainWindow��5	   	   $	��350��6   $	��928��6)    � 	Resources 5
      ��%    5      .��5      ]� 
���   �.+�5   
   � -   5        $��Gr��5      $��10q�6   $��Right=�6%   $	��200��6<   
���   � � ColumnDefinitions 5      �� 5      $	��100 �6#   61   �� 5      $
��Auto �6#   �� 5      $
��Auto �6#   �� 5      $
��Auto �6#   .+�5      ��  �� Orientation$ Vertical=�6   $��0��60   .+�5      �� $��20(�6   $��Center=�6&   
���������6V   �� *CurrentHr.H+�� 5      $��20(�6   $��Center=�6&   
���������6V   �� *CurrentHr.ElapsedTime+5      0   *WpfApplication2.CustomControls.LinearGauge  5      $��1��6)      	GaugeFill
 �  ��   GaugePercent 6I   �� *CurrentHr.RedPercent+   ActiveGauge 6w   �� *CurrentHr.RedActive+  5      $��2��6)   $��5,0,5,0q�69   
 ��  � 6[   �� *CurrentHr.BluePercent+ 6�   �� *CurrentHr.BlueActive+  5      $��3��6)   
 � � � 6K   �� *CurrentHr.GreenPercent+ 6|   �� *CurrentHr.GreenActive+5      �� 5   
   $ 
Horizontal=�6   $��Bottom=�6.   .+�5      �� -   5        $��Debug��5      6%   �� 5!      -   5        $��Connect��5!      6'   �� 5"      -   5        $��
Disconnect��5"      6*   �� 5#      -   5        $
��Save��5#      6$   �� 5$      
���������6   �� *Test+6-   5%      ]� 5'   
   .+�5(      �� $��Right=�6   $ 
Horizontal=�65   $��Bottom=�6N   .+�5)      �� 
�������$��
HR Average��6b    d� 	IsChecked 6.   �� *HrAverageVisible+ �� Mode$ TwoWay=�6y   �� 5*      
�������$��HR Max��6^    6.   �� *HrMaxVisible+$ TwoWay=�6q   �� 5+      
�������$��HR Min��6^    6.   �� *HrMinVisible+$ TwoWay=�6q   5,      '  !Syncfusion.UI.Xaml.Charts.SfChart  5.      -   5        
���   �$��10q�5.   /   	   	Behaviors	 51      6  0Syncfusion.UI.Xaml.Charts.ChartTrackBallBehavior  -   5        tt��
   	LineStyle
 52      �� 54       �� 
TargetType" ��6$   .�56   "   ��  �� Property � �� �� Value
 � ���6;   �� 58   "    � ��$ 1��6D   59      5;         Annotations 5>      8  2Syncfusion.UI.Xaml.Charts.HorizontalLineAnnotation  *  $Syncfusion.UI.Xaml.Charts.Annotation   CoordinateUnit$
 Axis=�6w   /  )Syncfusion.UI.Xaml.Charts.ShapeAnnotation   CanDrag .     
Foreground
 ��  �	   Y1 65   �� *	HrAverage+.  (Syncfusion.UI.Xaml.Charts.LineAnnotation   ShowLine 6N   �� *HrAverageVisible+6  0Syncfusion.UI.Xaml.Charts.StraightLineAnnotation   ShowAxisLabel 5?   D   �� *HrAverageVisible+  5@         Stroke
 � ���   StrokeDashArray$	 4,4R�6�   $
 Axis=�6�    .   65   �� *HrMin+ 6Z   �� *HrMinVisible+ 6�   �� *HrMinVisible+6�     5A      $
 Axis=�6�    .  
 � ���$	 4,4R�5B   A    5A   5   �� *HrMax+ 6J   �� *HrMaxVisible+ 6m   �� *HrMaxVisible+5B   A      PrimaryAxis 5D      ,	  &Syncfusion.UI.Xaml.Charts.TimeSpanAxis	  5E      )
  #Syncfusion.UI.Xaml.Charts.ChartAxis 
  ShowTrackBallInfo .  
������� 
  Header$
 Time��6V   $��12(�6d    	  Minimum$ 00:00:00X�6r    	  Maximum$ 00:03:00X�5F   %    
  MajorGridLineStyle 5G      �� 5H      " ��6$   .�5I   "   ��  � ��$ 1��6D   �� 5J   "    � �� �#2727276;   5K      5M         SecondaryAxis 5P      -  'Syncfusion.UI.Xaml.Charts.NumericalAxis  5Q      $ HR��6*   
�������   RangePadding$ 
Additional=�6I   $��12(�6c    5R      �� 5S      " ��6$   .�5T   "   ��  � ��$ 1��6D   �� 5U   "    � �� �#2727276;   5V      5X         Series. 5[      ,  &Syncfusion.UI.Xaml.Charts.SplineSeries  -	   5           Name$  h��5[   %   /  )Syncfusion.UI.Xaml.Charts.ChartSeriesBase!   EnableAnimation! . "   Interior" �#10E810+  %Syncfusion.UI.Xaml.Charts.ChartSeries#   StrokeThickness$	# 1.5Q�6Z   $   XBindingPath$$ ElapsedTime��5\   >   ,  &Syncfusion.UI.Xaml.Charts.XyDataSeries%   YBindingPath$% H��6Y   &   ItemsSource& 6!   �� *Data+5^      5_      5`      5a      5b      