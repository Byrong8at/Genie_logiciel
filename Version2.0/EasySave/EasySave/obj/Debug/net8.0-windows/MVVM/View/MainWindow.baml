   M S B A M L   `   `   `  ����C  ?EasySave, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null#clr-namespace:EasySaveEasySave  5   	       EasySave.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   (localclr-namespace:EasySave     5   	   -   5          � Title$  
MainWindow��5   	   $	��675��6   $
��1200��6)   ���#252525.��5      �  � ColumnDefinitions 5   
   �� 5      $	��225 �6   �� 5      .+�5   
   �� $��0��6   .+�5      �� $��65��5      $
��Bold"�5   C   $��13(�6U   $��0,0,0,20q�6c   
���������5      �� *currentMenuTitle+��5      ��  �� Path$ NavigateHomeCommand�6u   �� 5      $��65��5      $
��Bold"�5   G   $��13(�6Y   
���������5      �� *currentMenu1+��5      �� $ NavigateLanguageCommand�6g   �� 5      $��65��5      $
��Bold"�5   F   $��13(�6X   
���������5      �� *currentMenu2+��5      �� $ NavigateExecuteCommand�6f   �� 5!      $��65��5"      $
��Bold"�5#   E   $��13(�6W   
���������5!      �� *currentMenu3+��5#      �� $ NavigateCreateCommand�6e   �� 5&      $��65��5'      $
��Bold"�5(   G   $��13(�6Y   
���������5&      �� *currentMenu4+��5(      �� $ NavigateOverviewCommand�6g   �� 5+      $��65��5,      $
��Bold"�5-   E   $��13(�6W   
���������5+      �� *currentMenu5+��5-      �� $ NavigateDeleteCommand�6e   �� 50      $��65��6   $
��Bold"�51   D   $��13(�6V   
���������6   �� $ NavigateCheckCommand�.��52      �� -   5        $��textBox1��52      $��Center=�6*   $��13(�53   <   $��Center=�54      $��Center=�6@    �� 
IsReadOnly$
 True��6�    �� SelectionOpacity$ 1Q�6�   $|�False��6�   $��False��6   � Cursor$ Arrow��6  $��0,0,0,1q�6#  ��53      �� *currentMenu6+��54   d   Z� ��6z   Z�  �� SelectionBrush 6�   Z�  �� SelectionTextBrush 6�   Z� .��6=  ��55      �� 56       �� Color$ Black��6*   	 �� Opacity$	 1Q�68   .��57   *   58      59      5;      �� 5=   
   -   5        $��65��5>   	   $��Bottom=�5?   	   $
��Bold"�6;   $��13(�6M   
���������5=      �� *currentMenu7+5?   [   �� 5B   
   $��1��6   ��6)   �� *Navigation.CurrentView+5D      5F      5I      