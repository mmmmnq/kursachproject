   M S B A M L   `   `   `  ����D  @kursovaya, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null;"clr-namespace:kursovaya.ViewModelskursovaya.ViewModels  5   	   5clr-namespace:kursovaya.Helperskursovaya.Helpers  5   	       kursovaya.Views.UserForm   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   1vm"clr-namespace:kursovaya.ViewModels     5   	   3helpersclr-namespace:kursovaya.Helpers     5   	     � Title$  Туры��5   	   $	��700��5   	   $
��1000��5   	   ���#121212 � WindowStartupLocation$ CenterScreen=�5
   	    � WindowStyle$
 None=�5   	    � 
ResizeMode$ NoResize=�5   	    � AllowsTransparency .  � 	Resources 5      6   0kursovaya.Helpers.StatusToButtonEnabledConverter "  StatusToButtonEnabledConverter��%   &	          5   
   61   5       � DataContext 5      (   "kursovaya.ViewModels.TourViewModel  5   
   .��5      �� 
������ ���#333333$��1q�5       �� CornerRadius$ 20��5      $��10q�5      ._�5   
   �  � RowDefinitions 5      �� 5      $
��Auto �6    �� 5      $��* �6    �� 5       $
��Auto �6    .+�5$      �� $��0��6   ���#1E1E1E$ 	20,20,0,0��5&      	 �� Padding$	 20q�5'      ._�5(      � .+�5)      �� $!��Доступные туры��6    $��32(�5*      $��SemiBold"�5+      
�������$��Center=�5-      5.      5/      �� 52      $��1��6   $
��Auto=�53      $��20q�54      .��55      �� ��6   �� *Tours+��56      �� 57      .R�58      �� ���#444444$��1q�59   %   $ 15��5:   %   $��0,0,0,20q�5;   %   ���#1E1E1E  DropShadowEffect
 J� Effect#
 [ 5=   %   ._�5>   "   �  � ColumnDefinitions 5?   &   �� 5@   *   $	��300 �6;   �� 5A   *   $��* �6;   .+�5E   &   �� $��0��6-   $ 	15,0,0,15��5F   -   }�. ._�5H   *   �� $��UniformToFill=�5I   0   $	��200��5J   0   ��5H   0   �� *	ImagePath+5J   0   5K   '   � 5N   &   $��1��6+   $��20q�6;    5O   *   �� 5P   .   $
��Auto �6<   �� 5Q   .   $
��Auto �6<   �� 5R   .   $��* �6<   .+�5V   *   � $��0��6/   .+�5W   .   �� $
��Bold"�5X   6   $��24(�5Y   6   
���������5W   8   �� *Name+5Z   6   �� 5\   .   ���#2D2D2D$ 20��5]   5   $
	 15,5q�5^   5   $��Right=�5_   5   ._�5`   2   �� 
��� ���$��SemiBold"�5b   :   ��5`   <   �� *Status+5b   :   5c   /   5d   +   �� 5g   *   $��1��64   ���#4CAF50$
��Bold"�5j   2   $��20(�5k   2   $
��0,10q�5l   2   ��5h   2   �� *Price+ �� StringFormat$ Цена: {0:C}��5l   2   �� 5o   *   $��2��65    �� Orientation$ 
Horizontal=�5p   3   $��Bottom=�5q   3   .+�5r   .   �� $��Подробнее��65   $	��150��5s   5   $��45��5t   5     DetailsButtonStyle#��[ 5u   5   $��0,0,10,0q�5v   5   ��5w   5   �� *DataContext.DetailsCommand+ �� RelativeSource ��  �� AncestorType$ WindowL���5y   5   �� �� 5{   .   $ ��Забронировать��65   $	��150��5|   5   $��45��5}   5     BookButtonStyle#��[ 5~   5   ��5   5   �� *DataContext.BookCommand+ �� $ WindowL���5�   5   �� {�5�   5   �� *Status+ �� 	Converter# [  5�   +   5�   '   5�   #   5�      5�      5�      5�      �� 5�      $��2��6   ���#1E1E1E$ 	0,0,20,20��5�      $	 20q�5�      ._�5�      �� -   5         ��Name$ CloseButton��5�   	   $��Закрыть��5�      $	��120��5�   	   $��35��5�   	     AccentButtonStyle#��[ 5�   	   ���#4444445�   	   5�      5�      5�      5�      